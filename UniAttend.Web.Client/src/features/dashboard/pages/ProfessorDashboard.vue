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
                <Button v-if="!isSessionActive(session)" @click="startSession(session)" variant="primary"
                  :disabled="startingSessionId === session.studyGroupId">
                  {{ startingSessionId === session.studyGroupId ? 'Starting...' : 'Start Session' }}
                </Button>
                <Button v-else-if="isSessionActive(session) && !isSessionConfirmed(session)"
                  @click="confirmSession(session.id!)" variant="primary">
                  Confirm Attendance
                </Button>
                <Button v-else-if="isSessionActive(session) && session.id" @click="closeSession(session.id)"
                  variant="danger">
                  End Session
                </Button>
                <Badge :status="getSessionStatus(session)">
                  {{ session.status }}
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
import type {
  CourseSessionDto,
  AttendanceRecordDto,
  OpenCourseSessionCommand,
  TimeSpan
} from '@/api/generated/data-contracts'
import Button from '@/shared/components/ui/Button.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import AttendanceList from '@/features/attendance/components/AttendanceList.vue'

type BadgeStatus = 'success' | 'warning' | 'error' | 'info'

interface ExtendedCourseSession extends CourseSessionDto {
  isConfirmed: boolean;
  status: string;
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

const isLoading = ref(false)
const isLoadingRecords = ref(false)
const stats = ref<DashboardStats>({
  activeCourseSessions: 0,
  pendingConfirmations: 0,
  averageAttendance: 0
})

const attendanceActions = [
  {
    label: 'Mark Absent',
    icon: 'person_off',
    show: (record: AttendanceRecordDto) => !record.isConfirmed && !record.isAbsent,
    action: (record: AttendanceRecordDto) => markStudentAbsent(record)
  }
]

const recentRecords = ref<AttendanceRecordDto[]>([])
const todayScheduledSessions = ref<ExtendedCourseSession[]>([])
const currentSessionAttendance = ref<AttendanceRecordDto[]>([])
const isLoadingAttendance = ref(false)
const startingSessionId = ref<number | null>(null)
const activeSessionId = ref<number | null>(null)
const pollInterval = ref<number | null>(null)

interface CourseSessionQueryParams {
  studyGroupId?: number;
  classroomId?: number;
  date?: Date;
  professorId?: number;
}

// Utility functions
function formatTimeString(time: TimeSpan | undefined): string {
  if (!time) return '';

  // Ensure we have numbers for hours and minutes
  const hours = typeof time.hours === 'number' ? time.hours.toString().padStart(2, '0') : '00';
  const minutes = typeof time.minutes === 'number' ? time.minutes.toString().padStart(2, '0') : '00';

  return `${hours}:${minutes}`;
}

async function markStudentAbsent(record: AttendanceRecordDto) {
  if (!record.courseSessionId || !record.studentId) return;

  try {
    await attendanceStore.markAbsent(record.courseSessionId, record.studentId);
    await loadSessionAttendance(record.courseSessionId);
  } catch (err) {
    console.error('Failed to mark student as absent:', err);
  }
}

function isSessionActive(session: ExtendedCourseSession): boolean {
  return session.status?.toLowerCase() === 'active'
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
  const status = session.status?.toLowerCase()
  if (status === 'active') return 'success'
  if (status === 'scheduled') return 'info'
  return 'info'
}

// Session management functions
async function startSession(session: ExtendedCourseSession) {
  if (startingSessionId.value === session.studyGroupId) return
  startingSessionId.value = session.studyGroupId!

  try {
    const command: OpenCourseSessionCommand = {
      studyGroupId: session.studyGroupId!,
      classroomId: session.classroomId!,
      courseSessionId: 0,
      date: new Date().toISOString(),
      startTime: session.startTime!,
      endTime: session.endTime!
    }
    const newSession = await courseSessionStore.OpenCourseSession(command)
    if (newSession?.id) {
      session.id = newSession.id
      await loadSessionAttendance(newSession.id)
      startAttendancePolling(newSession.id) // Start polling
    }
    await loadDashboardData()
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

async function loadDashboardData() {
  isLoading.value = true
  try {
    // Ensure we're using the start of today for proper date comparison
    const today = new Date()
    today.setHours(0, 0, 0, 0)

    const [schedules, sessions, attendanceStats] = await Promise.all([
      scheduleStore.fetchSchedules(undefined, undefined, authStore.user?.id),
      courseSessionStore.fetchCourseSessions({
        date: today,
        professorId: authStore.user?.id
      } as CourseSessionQueryParams), // Cast to the correct type
      attendanceStore.fetchAttendance()
    ])

    console.log('Fetched schedules:', schedules)
    console.log('Fetched sessions:', sessions)

    // Filter schedules for today
    const todaySchedules = schedules.filter(schedule => {
      const scheduleDay = new Date().getDay() // 0 = Sunday, 1 = Monday, etc.
      return schedule.dayOfWeek === scheduleDay // Assuming schedule has dayOfWeek property
    })

    // Clear previous sessions
    todayScheduledSessions.value = []

    // Map schedules to sessions without duplicates
    const processedGroups = new Set()

    todaySchedules.forEach(schedule => {
      if (processedGroups.has(schedule.studyGroupId)) return;
      processedGroups.add(schedule.studyGroupId);

      // Find matching active session
      const activeSession = sessions?.find(s =>
        s.studyGroupId === schedule.studyGroupId &&
        s.classroomId === schedule.classroomId
      );

      // Create the session object with explicit typing
      const sessionObject: ExtendedCourseSession = {
        ...schedule,
        id: activeSession?.id,
        studyGroupId: schedule.studyGroupId,
        studyGroupName: schedule.studyGroupName,
        classroomId: schedule.classroomId,
        classroomName: schedule.classroomName,
        startTime: schedule.startTime,
        endTime: schedule.endTime,
        status: activeSession?.status || 'scheduled',
        isConfirmed: activeSession?.status === 'completed' || false
      };

      if (activeSession?.status?.toLowerCase() === 'active') {
        startAttendancePolling(activeSession.id!);
        loadSessionAttendance(activeSession.id!);
      }

      todayScheduledSessions.value.push(sessionObject);
    });

    // Update stats
    stats.value = {
      activeCourseSessions: sessions?.filter(s => s.status?.toLowerCase() === 'active').length || 0,
      pendingConfirmations: sessions?.filter(s => {
        const extendedSession = s as ExtendedCourseSession
        return !extendedSession.isConfirmed && s.status?.toLowerCase() === 'active'
      }).length || 0,
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
      recentRecords.value = reportData.dailyRecords.map((day: {
        date: string;
        totalCourseSessions: number;
        attendanceRate: number;
      }) => ({
        checkInTime: day.date,
        isConfirmed: true,
        courseName: `${day.totalCourseSessions} sessions`,
        professor: `${day.attendanceRate}% attendance`
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