<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Professor Dashboard</h1>
    </div>

    <!-- Stats Overview -->
    <div class="grid grid-cols-3 gap-4">
      <StatCard title="Active Course Sessions" :value="stats.activeCourseSessions" subtitle="Today's Sessions" />
      <StatCard title="Pending Confirmations" :value="stats.pendingConfirmations"
        :status="stats.pendingConfirmations > 0 ? 'warning' : 'success'" />
      <StatCard title="Attendance Rate" :value="`${stats.averageAttendance}%`" subtitle="Today's Average" />
    </div>

    <!-- Today's Sessions Section -->
    <div class="bg-white p-6 rounded-lg shadow">
      <h2 class="text-lg font-medium mb-4">Today's Course Sessions</h2>
      <div class="space-y-4">
        <!-- Loading state -->
        <div v-if="isLoading" class="flex justify-center py-8">
          <Spinner :size="6" />
        </div>

        <!-- Empty state -->
        <div v-else-if="!todayScheduledSessions.length" class="text-center py-8 text-gray-500">
          <span class="material-icons text-4xl mb-2">event_busy</span>
          <p>No course sessions scheduled for today</p>
        </div>

        <template v-for="session in todayScheduledSessions" :key="session.id">
          <div class="p-4 border rounded-lg">
            <!-- Session Info -->
            <div class="flex justify-between items-center mb-4">
              <div>
                <p class="font-medium">{{ session.studyGroupName }}</p>
                <p class="text-sm text-gray-500">
                  {{ formatTimeString(session.startTime) }} - {{ formatTimeString(session.endTime) }}
                  <span class="ml-2">{{ session.classroomName }}</span>
                </p>
              </div>
              <div class="flex gap-2">
                <Button v-if="!isSessionActive(session)" @click="startSession(session)" variant="primary" :disabled="startingSessionId === session.studyGroupId ||
                  todayScheduledSessions.some(s =>
                    s.studyGroupId === session.studyGroupId &&
                    s.status === SessionStatus.Active
                  )">
                  {{ startingSessionId === session.studyGroupId ? 'Starting...' : 'Start Session' }}
                </Button>

                <!-- Show End Session whenever session is active -->
                <Button v-if="isSessionActive(session) && session.id" @click="closeSession(session.id)"
                  variant="danger">
                  End Session
                </Button>

                <!-- Show Confirm Attendance if not confirmed -->
                <Button v-if="isSessionActive(session) && !isSessionConfirmed(session)"
                  @click="confirmSession(session.id!)" variant="primary">
                  Confirm Attendance
                </Button>

                <Badge :status="getSessionStatus(session)">
                  {{ getSessionStatusText(session) }}
                </Badge>
              </div>
            </div>

            <!-- Attendance List when session is active -->
            <AttendanceList :records="getSessionAttendance(session)" :loading="isLoadingAttendance"
              :empty-message="'No attendance records yet'" :actions="attendanceActions" compact />
          </div>
        </template>
      </div>
    </div>


  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useAttendanceStore } from '@/stores/attendance.store'
import { useReportStore } from '@/stores/report.store'
import { useCourseSessionStore } from '@/stores/courseSession.store'
import { useScheduleStore } from '@/stores/schedule.store'
import { useAuthStore } from '@/stores/auth.store'
import { useGroupStore } from '@/stores/studyGroup.store'
import type {
  CourseSessionDto,
  AttendanceRecordDto,
  TimeSpan,
  SessionStatus as ApiSessionStatus
} from '@/api/generated/data-contracts'
import Button from '@/shared/components/ui/Button.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import AttendanceList from '@/features/attendance/components/AttendanceList.vue'

type BadgeStatus = 'success' | 'warning' | 'error' | 'info'

enum SessionStatus {
  Scheduled = 0,
  Active = 1,
  Completed = 2,
  Closed = 3
}

interface ExtendedCourseSession extends Omit<CourseSessionDto, 'status'> {
  isConfirmed: boolean;
  status: SessionStatus;
  id?: number;
}

interface DashboardStats {
  activeCourseSessions: number
  pendingConfirmations: number
  averageAttendance: number
}

const attendanceStore = useAttendanceStore()
const reportStore = useReportStore()
const courseSessionStore = useCourseSessionStore()
const scheduleStore = useScheduleStore()
const authStore = useAuthStore()
const groupStore = useGroupStore()

const isLoading = ref(false)
const isLoadingRecords = ref(false)
const stats = ref<DashboardStats>({
  activeCourseSessions: 0,
  pendingConfirmations: 0,
  averageAttendance: 0
})

const attendanceActions: Array<{
  label: string
  icon?: string
  show?: (record: AttendanceRecordDto) => boolean
  action: (record: AttendanceRecordDto) => void | Promise<void>
}> = [
    {
      label: 'Mark Absent',
      icon: 'person_off',
      show: (record: AttendanceRecordDto): boolean => {
        const sessionStudents = enrolledStudents.value.get(record.courseSessionId || 0)
        return !record.isConfirmed &&
          !record.isAbsent &&
          Boolean(sessionStudents?.includes(record.studentId?.toString() || ''))
      },
      action: async (record: AttendanceRecordDto) => {
        if (!record.courseSessionId || !record.studentId) return
        try {
          await attendanceStore.markAbsent(record.courseSessionId, record.studentId)
          await loadSessionAttendance(record.courseSessionId)
        } catch (err) {
          console.error('Failed to mark student as absent:', err)
        }
      }
    }
  ]

async function loadEnrolledStudents(studyGroupId: number) {
  try {
    const students = await groupStore.fetchGroupStudents(studyGroupId)
    enrolledStudents.value.set(studyGroupId, students.map(s => s.studentId?.toString() || ''))
  } catch (err) {
    console.error('Failed to load enrolled students:', err)
  }
}

const recentRecords = ref<AttendanceRecordDto[]>([])
const todayScheduledSessions = ref<ExtendedCourseSession[]>([])
const currentSessionAttendance = ref<AttendanceRecordDto[]>([])
const isLoadingAttendance = ref(false)
const startingSessionId = ref<number | null>(null)
const activeSessionId = ref<number | null>(null)
const pollInterval = ref<number | null>(null)
const activeSessionsMap = ref(new Map<number, boolean>())
const enrolledStudents = ref<Map<number, string[]>>(new Map())

// Utility functions
function formatTimeString(time: TimeSpan | undefined): string {
  if (!time) return '';

  // Ensure we have numbers for hours and minutes
  const hours = typeof time.hours === 'number' ? time.hours.toString().padStart(2, '0') : '00';
  const minutes = typeof time.minutes === 'number' ? time.minutes.toString().padStart(2, '0') : '00';

  return `${hours}:${minutes}`;
}

function getSessionStatusText(session: ExtendedCourseSession): string {
  switch (session.status) {
    case SessionStatus.Active:
      return 'In Progress';
    case SessionStatus.Completed:
    case SessionStatus.Closed:
      return 'Completed';
    case SessionStatus.Scheduled:
      return 'Scheduled';
    default:
      return 'Unknown';
  }
}

function isSessionActive(session: ExtendedCourseSession): boolean {
  return session.status === SessionStatus.Active;
}

function isSessionConfirmed(session: ExtendedCourseSession): boolean {
  return session.isConfirmed || false
}

async function confirmSession(sessionId: number) {
  try {
    await attendanceStore.confirmAttendance(sessionId)
    await loadDashboardData()
  } catch (err) {
    console.error('Failed to confirm session:', err)
  }
}

function getSessionStatus(session: CourseSessionDto | ExtendedCourseSession): BadgeStatus {
  const status = typeof session.status === 'number' ? session.status : SessionStatus.Scheduled;
  switch (status) {
    case SessionStatus.Active:
      return 'warning'; // Active sessions should be warning to draw attention
    case SessionStatus.Completed:
    case SessionStatus.Closed:
      return 'success'; // Completed/closed sessions should be success
    case SessionStatus.Scheduled:
      return 'info';
    default:
      return 'info';
  }
}

// Session management functions
async function startSession(session: ExtendedCourseSession) {
  if (startingSessionId.value === session.studyGroupId) return
  if (isSessionActive(session)) return

  startingSessionId.value = session.studyGroupId!

  try {
    // Wrap command in a command object as expected by API
    const data = {
      command: {
        studyGroupId: session.studyGroupId!,
        classroomId: session.classroomId!,
        courseSessionId: 0,
        date: new Date().toISOString(),
        startTime: {
          hours: session.startTime?.hours || 0,
          minutes: session.startTime?.minutes || 0,
          seconds: 0,
        },
        endTime: {
          hours: session.endTime?.hours || 0,
          minutes: session.endTime?.minutes || 0,
          seconds: 0,
        }
      }
    }

    const newSession = await courseSessionStore.OpenCourseSession(data.command)
    if (!newSession?.id) throw new Error('Failed to create session')

    // Immediately update local state
    const sessionIndex = todayScheduledSessions.value.findIndex(
      s => s.studyGroupId === session.studyGroupId
    )

    if (sessionIndex !== -1) {
      await loadEnrolledStudents(session.studyGroupId!)
      const updatedSession: ExtendedCourseSession = {
        ...todayScheduledSessions.value[sessionIndex],
        id: newSession.id,
        status: SessionStatus.Active,
        isConfirmed: false
      }
      todayScheduledSessions.value[sessionIndex] = updatedSession

      // Start attendance tracking
      await loadSessionAttendance(newSession.id)
      startAttendancePolling(newSession.id)

      // Update dashboard stats
      stats.value = {
        ...stats.value,
        activeCourseSessions: stats.value.activeCourseSessions + 1,
        pendingConfirmations: stats.value.pendingConfirmations + 1
      }
    }
  } catch (err) {
    console.error('Failed to start session:', err)
  } finally {
    startingSessionId.value = null
  }
}

function getSessionAttendance(session: ExtendedCourseSession): AttendanceRecordDto[] {
  if (!isSessionActive(session)) return []
  if (session.id !== activeSessionId.value) return []
  console.log('Getting attendance for session:', session.id, currentSessionAttendance.value)
  return currentSessionAttendance.value || []
}

async function loadSessionAttendance(sessionId: number) {
  console.log('Loading attendance for session:', sessionId)
  isLoadingAttendance.value = true

  try {
    const records = await attendanceStore.fetchClassAttendance(sessionId)
    console.log('Raw attendance records:', records)

    if (records) {
      currentSessionAttendance.value = Array.isArray(records) ? records : []
      console.log('Updated attendance records:', currentSessionAttendance.value)
    } else {
      console.warn('No records returned')
      currentSessionAttendance.value = []
    }
  } catch (err) {
    console.error('Failed to load session attendance:', err)
    currentSessionAttendance.value = []
  } finally {
    isLoadingAttendance.value = false
  }
}

// Add polling function
function startAttendancePolling(sessionId: number) {
  console.log('Starting attendance polling for session:', sessionId)
  stopAttendancePolling()
  activeSessionId.value = sessionId

  pollInterval.value = window.setInterval(async () => {
    if (activeSessionId.value) {
      await loadSessionAttendance(activeSessionId.value)
    }
  }, 30000) // Poll every 30 seconds
}

function stopAttendancePolling() {
  if (pollInterval.value) {
    clearInterval(pollInterval.value)
    pollInterval.value = null
  }
  activeSessionId.value = null
}

async function closeSession(sessionId: number) {
  try {
    await courseSessionStore.closeCourseSession(sessionId)
    stopAttendancePolling()
    await loadDashboardData()
  } catch (err) {
    console.error('Failed to close session:', err)
  }
}

function parseTime(timeStr: string | undefined): { hours: number; minutes: number } | undefined {
  if (!timeStr) return undefined;
  const parts = timeStr.split(':');
  if (parts.length < 2) return undefined;
  const hours = parseInt(parts[0], 10);
  const minutes = parseInt(parts[1], 10);
  return { hours, minutes };
}

async function loadDashboardData() {
  isLoading.value = true
  try {
    const today = new Date()

    // 1. Fetch all required data
    const [schedules, todaySessions, attendanceStats] = await Promise.all([
      scheduleStore.fetchSchedules(undefined, undefined, undefined, authStore.user?.id),
      courseSessionStore.fetchCourseSessions({
        date: today,
        professorId: authStore.user?.id
      }),
      attendanceStore.fetchAttendance()
    ])

    // 2. Clear previous state
    todayScheduledSessions.value = []

    // 3. Handle all today's sessions (active, completed, and closed)
    todaySessions?.forEach((session: CourseSessionDto) => {
      if (session.studyGroupId !== undefined) {
        activeSessionsMap.value.set(session.studyGroupId, true)
        const sessionObject: ExtendedCourseSession = {
          ...session,
          status: (typeof session.status === 'number'
            ? (session.status as ApiSessionStatus as unknown as SessionStatus)
            : SessionStatus.Scheduled),
          isConfirmed: false
        }
        todayScheduledSessions.value.push(sessionObject)

        if (session.id && sessionObject.status === SessionStatus.Active) {
          startAttendancePolling(session.id)
          loadSessionAttendance(session.id)
        }
      }
    })

    // 4. Add remaining scheduled sessions that haven't started yet
    const todaySchedules = schedules.filter(schedule => {
      const scheduleDay = new Date().getDay()
      return schedule.dayOfWeek === scheduleDay &&
        schedule.studyGroupId !== undefined &&
        !activeSessionsMap.value.get(schedule.studyGroupId)
    })

    todaySchedules.forEach(schedule => {
      const sessionObject: ExtendedCourseSession = {
        ...schedule,
        startTime: parseTime(schedule.startTime as unknown as string),
        endTime: parseTime(schedule.endTime as unknown as string),
        id: undefined,
        status: SessionStatus.Scheduled,
        isConfirmed: false
      }
      todayScheduledSessions.value.push(sessionObject)
    })

    // 5. Sort sessions by time
    todayScheduledSessions.value.sort((a, b) => {
      const timeA = a.startTime?.hours || 0
      const timeB = b.startTime?.hours || 0
      return timeA - timeB
    })

    // 6. Update stats
    stats.value = {
      activeCourseSessions: todayScheduledSessions.value.filter(s =>
        s.status === SessionStatus.Active
      ).length,
      pendingConfirmations: todayScheduledSessions.value.filter(s =>
        s.status === SessionStatus.Active && !s.isConfirmed
      ).length,
      averageAttendance: calculateAverageAttendance(attendanceStats)
    }

  } catch (err) {
    console.error('Failed to load dashboard data:', err)
  } finally {
    isLoading.value = false
  }
}

function calculateAverageAttendance(records: AttendanceRecordDto[]): number {
  if (!records || !records.length) return 0;
  const confirmed = records.filter(r => r.isConfirmed).length;
  return Math.round((confirmed / records.length) * 100);
}

async function loadRecentRecords() {
  isLoadingRecords.value = true;
  try {
    const today = new Date();
    const lastWeek = new Date(today);
    lastWeek.setDate(lastWeek.getDate() - 7);

    const reportData = await reportStore.getAttendanceReport({
      startDate: lastWeek,
      endDate: today
    });

    if (reportData?.dailyRecords) {
      recentRecords.value = reportData.dailyRecords.map(value => ({
        checkInTime: value.date?.toString(),
        isConfirmed: true,
        courseName: `${value.totalCourseSessions} sessions`,
        professor: `${value.attendanceRate}% attendance`
      }));
    }
  } catch (err) {
    console.error('Failed to load recent records:', err);
  } finally {
    isLoadingRecords.value = false;
  }
}

onMounted(() => {
  loadDashboardData()
  loadRecentRecords()
})

onUnmounted(() => {
  stopAttendancePolling()
})
</script>