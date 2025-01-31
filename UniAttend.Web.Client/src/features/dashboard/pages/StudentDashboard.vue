<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Student Dashboard</h1>
    </div>

    <!-- Personal Stats -->
    <div class="grid grid-cols-3 gap-4">
      <StatCard title="Total Course Sessions" :value="Number(attendanceStats.totalCourseSessions || 0)" />
      <StatCard title="Attended Sessions" :value="Number(attendanceStats.attendedCourseSessions || 0)" />
      <StatCard title="Absence Rate" :value="`${absenceRate}%`" :status="getAbsenceStatus(absenceRate)" />
    </div>

    <!-- Content Grid -->
    <div class="space-y-6">
      <!-- Today's Active Sessions -->
      <div class="bg-white p-6 rounded-lg shadow">
        <h2 class="text-lg font-medium mb-4">Today's Active Sessions</h2>
        <div class="space-y-4">
          <div v-if="isLoadingSchedule" class="flex justify-center">
            <Spinner :size="6" />
          </div>
          <div v-else-if="!todayActiveSessions.length" class="text-gray-500 text-center">
            No active sessions for today
          </div>
          <div v-else class="grid gap-4 md:grid-cols-2">
            <div v-for="session in todayActiveSessions" :key="session.id"
              class="flex flex-col p-4 bg-gray-50 rounded-md space-y-4">
              <div class="flex justify-between items-start">
                <div>
                  <p class="font-medium">{{ session.studyGroupName }}</p>
                  <p class="text-sm text-gray-500">
                    {{ formatTime(session.startTime) }} - {{ session.classroomName }}
                  </p>
                </div>
                <Badge :status="getSessionStatus(session)">
                  {{ getSessionStatusText(session) }}
                </Badge>
              </div>

              <!-- Add check-in options when session is active -->
              <div v-if="session.status === 'Active'" class="flex gap-2">
                <Button @click="router.push(`/dashboard/attendance/otp-check-in/${session.id}`)" variant="secondary"
                  size="sm">
                  Check-in with OTP
                </Button>
                <Button @click="router.push(`/dashboard/attendance/check-in/${session.id}`)" size="sm">
                  Check-in with Card
                </Button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Enrolled Groups -->
      <div class="bg-white p-6 rounded-lg shadow">
        <h2 class="text-lg font-medium mb-4">My Enrolled Groups</h2>
        <div class="space-y-4">
          <div v-if="isLoadingGroups" class="flex justify-center">
            <Spinner :size="6" />
          </div>
          <div v-else-if="!enrolledGroups.length" class="text-gray-500 text-center">
            No enrolled groups
          </div>
          <div v-else class="grid gap-4 md:grid-cols-2">
            <div v-for="group in enrolledGroups" :key="group.studyGroupId" class="p-4 bg-gray-50 rounded-md space-y-2">
              <div class="flex justify-between items-start">
                <div>
                  <p class="font-medium">{{ group.studyGroupName }}</p>
                  <p class="text-sm text-gray-600">{{ group.subjectName }}</p>
                </div>
                <Badge status="info">
                  {{ group.academicYearName }}
                </Badge>
              </div>
              <div class="flex items-center text-sm text-gray-600">
                <span class="material-icons-outlined text-sm mr-1">person</span>
                {{ group.professorName }}
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Recent Attendance - Now full width -->
      <div class="bg-white p-6 rounded-lg shadow">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-lg font-medium">Recent Attendance</h2>
          <Button variant="secondary" size="sm" @click="router.push('/dashboard/attendance/view')">
            View All
          </Button>
        </div>
        <div class="space-y-4">
          <div v-if="isLoadingAttendance" class="flex justify-center">
            <Spinner :size="6" />
          </div>
          <div v-else>
            <AttendanceList :records="recentAttendance" :loading="false" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useStudentStore } from '@/stores/student.store'
import { useAttendanceStore } from '@/stores/attendance.store'
import { useCourseSessionStore } from '@/stores/courseSession.store'
import type { TimeSpan } from '@/api/generated/data-contracts'
import type {
  CourseSessionDto,
  AttendanceRecordDto,
  UserGroupDto,
  AttendanceStatsDto
} from '@/api/generated/data-contracts'
import Button from '@/shared/components/ui/Button.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import AttendanceList from '@/features/attendance/components/AttendanceList.vue'

const router = useRouter()
const studentStore = useStudentStore()
const attendanceStore =useAttendanceStore()
const courseSessionStore = useCourseSessionStore()

// State
const isLoadingSchedule = ref(false)
const isLoadingAttendance = ref(false)
const attendanceStats = ref<AttendanceStatsDto>({
  totalCourseSessions: 0,
  attendedCourseSessions: 0,
  attendanceRate: 0,
})

const recentAttendance = ref<AttendanceRecordDto[]>([])
const isLoadingGroups = ref(false)
const enrolledGroups = ref<UserGroupDto[]>([])
const todayActiveSessions = ref<CourseSessionDto[]>([])

// Computed
const absenceRate = computed(() => {
  if (!attendanceStats.value.totalCourseSessions) return 0
  return Math.round(100 - (attendanceStats.value.attendanceRate || 0))
})

// Utility functions
function getAbsenceStatus(rate: number): 'success' | 'warning' | 'error' {
  if (rate <= 20) return 'success'
  if (rate <= 30) return 'warning'
  return 'error'
}

function getSessionStatus(session: CourseSessionDto): 'success' | 'warning' | 'info' {
  if (session.status === 'Completed') return 'success'
  if (session.status === 'Active') return 'warning'
  return 'info'
}

function getSessionStatusText(session: CourseSessionDto): string {
  switch (session.status?.toLowerCase()) {
    case 'completed':
      return 'Attended'
    case 'active':
      return 'In Progress'
    default:
      return 'Upcoming'
  }
}

function formatTime(time: TimeSpan | undefined): string {
  if (!time || time.hours === undefined || time.minutes === undefined) {
    return '--:--'
  }
  return `${time.hours.toString().padStart(2, '0')}:${time.minutes.toString().padStart(2, '0')}`
}

// Data loading
async function loadDashboardData() {
  try {
    // Load attendance stats
    const stats = await attendanceStore.fetchAttendanceStudentList()
    if (stats.length > 0) {
      attendanceStats.value = {
        totalCourseSessions: stats[0].courseSessionId || 0,
        attendedCourseSessions: stats.length,
        attendanceRate: (stats.length / (stats[0].courseSessionId || 1)) * 100
      }
    }
  } catch (err) {
    console.error('Failed to load dashboard stats:', err)
  }
}

async function loadEnrolledGroups() {
  isLoadingGroups.value = true
  try {
    const response = await studentStore.fetchStudentGroups()
    enrolledGroups.value = Array.isArray(response) ? response : []
  } catch (err) {
    console.error('Failed to load enrolled groups:', err)
    enrolledGroups.value = []
  } finally {
    isLoadingGroups.value = false
  }
}

async function loadTodayActiveSessions() {
  isLoadingSchedule.value = true
  try {
    const today = new Date()
    const sessions = await courseSessionStore.fetchCourseSessions({
      date: today
    })
    // Update the filter to use correct property names from UserGroupDto
    todayActiveSessions.value = sessions.filter(session =>
      enrolledGroups.value.some(group => group.studyGroupId === session.studyGroupId)
    )
  } catch (err) {
    console.error('Failed to load today\'s sessions:', err)
  } finally {
    isLoadingSchedule.value = false
  }
}

async function loadRecentAttendance() {
  isLoadingAttendance.value = true
  try {
    const records = await attendanceStore.fetchAttendanceStudentList()
    recentAttendance.value = records
  } catch (err) {
    console.error('Failed to load attendance:', err)
  } finally {
    isLoadingAttendance.value = false
  }
}

onMounted(async () => {
  await loadEnrolledGroups()
  await Promise.all([
    loadDashboardData(),
    loadTodayActiveSessions(),
    loadRecentAttendance()
  ])
})
</script>