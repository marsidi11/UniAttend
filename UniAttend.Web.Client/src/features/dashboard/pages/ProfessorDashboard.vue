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
                <Button v-if="!isSessionActive(session)" @click="startSession(session)" variant="primary">
                  Start Session
                </Button>
                <Button v-else-if="!isSessionConfirmed(session)" @click="confirmSession(session.id!)" variant="primary">
                  Confirm Attendance
                </Button>
                <Button v-else-if="isSessionActive(session) && session.id" @click="closeSession(session.id)" variant="danger">
                  End Session
                </Button>
                <Badge :status="getSessionStatus(session)">
                  {{ session.status }}
                </Badge>
              </div>
            </div>

            <!-- Attendance List when session is active -->
            <div v-if="isSessionActive(session)" class="mt-4">
              <AttendanceList :records="currentSessionAttendance" :loading="isLoadingAttendance" compact />
            </div>
          </div>
        </template>
      </div>
    </div>


  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
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
  isConfirmed?: boolean;
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

const recentRecords = ref<AttendanceRecordDto[]>([])
const todayScheduledSessions = ref<ExtendedCourseSession[]>([])
const currentSessionAttendance = ref<AttendanceRecordDto[]>([])
const isLoadingAttendance = ref(false)

// Utility functions
function formatTimeString(time: TimeSpan | undefined): string {
  if (!time) return ''
  const hours = time.hours?.toString().padStart(2, '0') || '00'
  const minutes = time.minutes?.toString().padStart(2, '0') || '00'
  return `${hours}:${minutes}`
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

function getSessionStatus(session: CourseSessionDto): BadgeStatus {
  if (isSessionActive(session)) return 'success'
  if (session.status?.toLowerCase() === 'scheduled') return 'info'
  return 'info'
}

// Session management functions
async function startSession(session: ExtendedCourseSession) {
  try {
    const command: OpenCourseSessionCommand = {
      studyGroupId: session.studyGroupId!,
      classroomId: session.classroomId!,
      courseSessionId: 0,
      date: new Date().toISOString(),
      startTime: session.startTime!,
      endTime: session.endTime!
    }
    await courseSessionStore.OpenCourseSession(command)
    await loadSessionAttendance(session.id!)
    await loadDashboardData()
  } catch (err) {
    console.error('Failed to start session:', err)
  }
}

async function loadSessionAttendance(sessionId: number) {
  isLoadingAttendance.value = true
  try {
    const records = await attendanceStore.fetchClassAttendance(sessionId)
    currentSessionAttendance.value = records || []
  } catch (err) {
    console.error('Failed to load session attendance:', err)
  } finally {
    isLoadingAttendance.value = false
  }
}

async function closeSession(sessionId: number) {
  try {
    await courseSessionStore.closeCourseSession(sessionId)
    await loadDashboardData()
  } catch (err) {
    console.error('Failed to close session:', err)
  }
}

async function loadDashboardData() {
  isLoading.value = true
  try {
    const today = new Date()
    const [schedules, sessions, attendanceStats] = await Promise.all([
      // Get professor's schedule for today
      scheduleStore.fetchSchedules(undefined, undefined, authStore.user?.id), // professor's schedule
      courseSessionStore.fetchCourseSessions({ date: today }),
      attendanceStore.fetchAttendance()
    ])

    // Map schedules to sessions
    todayScheduledSessions.value = schedules.map(schedule => ({
      id: undefined, // Will be set when session starts
      studyGroupId: schedule.studyGroupId,
      studyGroupName: schedule.studyGroupName,
      classroomId: schedule.classroomId,
      classroomName: schedule.classroomName,
      startTime: schedule.startTime,
      endTime: schedule.endTime,
      status: 'scheduled',
      isConfirmed: false
    }))

    // Update with any active sessions
    if (sessions?.length) {
      todayScheduledSessions.value = todayScheduledSessions.value.map(scheduled => {
        const active = sessions.find(s => 
          s.studyGroupId === scheduled.studyGroupId && 
          s.classroomId === scheduled.classroomId
        )
        return active ? { ...scheduled, ...active } : scheduled
      })
    }

    // Update stats
    stats.value = {
      activeCourseSessions: sessions?.filter(s => isSessionActive(s)).length || 0,
      pendingConfirmations: sessions?.filter(s => !(s as ExtendedCourseSession).isConfirmed).length || 0,
      averageAttendance: calculateAverageAttendance(attendanceStats)
    }
  } catch (err) {
    console.error('Failed to load dashboard data:', err)
  } finally {
    isLoading.value = false
  }
}

function calculateAverageAttendance(records: AttendanceRecordDto[]): number {
  if (!records.length) return 0
  const confirmed = records.filter(r => r.isConfirmed).length
  return Math.round((confirmed / records.length) * 100)
}


async function loadRecentRecords() {
  isLoadingRecords.value = true
  try {
    const today = new Date()
    const lastWeek = new Date(today)
    lastWeek.setDate(lastWeek.getDate() - 7)

    const reportData = await reportStore.getAttendanceReport({
      startDate: lastWeek,
      endDate: today
    })

    if (reportData?.dailyRecords) {
      recentRecords.value = reportData.dailyRecords.map(day => ({
        checkInTime: day.date,
        isConfirmed: true,
        courseName: `${day.totalCourseSessions} sessions`,
        professor: `${day.attendanceRate}% attendance`
      }))
    }
  } catch (err) {
    console.error('Failed to load recent records:', err)
  } finally {
    isLoadingRecords.value = false
  }
}

onMounted(() => {
  loadDashboardData()
  loadRecentRecords()
})
</script>