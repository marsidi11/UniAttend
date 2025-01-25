<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Professor Dashboard</h1>
      <Button @click="OpenCourseSessionSession" variant="primary">
        Open Class Session
      </Button>
    </div>

    <!-- Stats Overview -->
    <div class="grid grid-cols-3 gap-4">
      <StatCard title="Active courseSessions" :value="stats.activeCourseSessions" subtitle="Today's Sessions" />
      <StatCard title="Pending Confirmations" :value="stats.pendingConfirmations"
        :status="stats.pendingConfirmations > 0 ? 'warning' : 'success'" />
      <StatCard title="Attendance Rate" :value="`${stats.averageAttendance}%`" subtitle="Today's Average" />
    </div>

    <!-- Content Grid -->
    <div class="grid grid-cols-2 gap-4">
      <!-- Today's courseSessions -->
      <div class="bg-white p-6 rounded-lg shadow">
        <h2 class="text-lg font-medium mb-4">Today's courseSessions</h2>
        <div class="space-y-4">
          <div v-if="isLoading" class="flex justify-center">
            <Spinner :size="6" />
          </div>
          <div v-else-if="!todaycourseSessions.length" class="text-gray-500 text-center">
            No courseSessions scheduled for today
          </div>
          <template v-else>
            <div v-for="classItem in todaycourseSessions" :key="classItem.id">
              <p class="font-medium">{{ classItem.groupName }}</p>
              <p class="text-sm text-gray-500">
                {{ formatDate(new Date(classItem.date || '')) }} - {{ classItem.classroomName }}
              </p>
              <Badge :status="getStatusBadgeType(classItem.status || '')">
                {{ classItem.status }}
              </Badge>
              <div class="flex space-x-2">
                <Button v-if="classItem.status === 'active' && classItem.id" @click="confirmAttendance(classItem.id)"
                  variant="primary">
                  Confirm Attendance
                </Button>
                <Button v-if="classItem.id" @click="viewAttendance(classItem.id)" variant="secondary">
                  View Details
                </Button>
              </div>
            </div>
          </template>
        </div>
      </div>
    </div>

    <!-- Recent Attendance Records -->
    <div class="bg-white p-6 rounded-lg shadow">
      <div class="flex justify-between items-center mb-4">
        <h2 class="text-lg font-medium">Recent Attendance</h2>
        <Button @click="router.push('/dashboard/attendance/records')" variant="secondary">
          View All
        </Button>
      </div>
      <div class="space-y-4">
        <div v-if="isLoadingRecords" class="flex justify-center">
          <Spinner :size="6" />
        </div>
        <div v-else>
          <AttendanceList :records="recentRecords" :loading="false" compact />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAttendanceStore } from '@/stores/attendance.store'
import { useReportStore } from '@/stores/report.store'
import { useCourseSessiontore } from '@/stores/courseSession.store'
import type {
  CourseSessionDto,
  AttendanceRecordDto
} from '@/api/generated/data-contracts'
import { formatDate } from '@/utils/dateUtils'
import Button from '@/shared/components/ui/Button.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import AttendanceList from '@/features/attendance/components/AttendanceList.vue'

type BadgeStatus = 'success' | 'warning' | 'error' | 'info'

interface DashboardStats {
  activeCourseSessions: number
  pendingConfirmations: number
  averageAttendance: number
}

const router = useRouter()
const attendanceStore = useAttendanceStore()
const reportStore = useReportStore()
const classStore = useCourseSessiontore()

const isLoading = ref(false)
const isLoadingRecords = ref(false)
const stats = ref<DashboardStats>({
  activeCourseSessions: 0,
  pendingConfirmations: 0,
  averageAttendance: 0
})
const todaycourseSessions = ref<CourseSessionDto[]>([])
const recentRecords = ref<AttendanceRecordDto[]>([])

function getStatusBadgeType(status: string): BadgeStatus {
  switch (status?.toLowerCase()) {
    case 'active': return 'success'
    case 'pending': return 'warning'
    default: return 'info'
  }
}

async function loadDashboardData() {
  isLoading.value = true
  try {
    const today = new Date()
    const tomorrow = new Date(today)
    tomorrow.setDate(tomorrow.getDate() + 1)

    const [courseSessionsData, reportData] = await Promise.all([
      classStore.fetchCourseSessions({ date: today }), // Pass object with date property
      reportStore.getAttendanceReport({
        startDate: today,
        endDate: tomorrow
      })
    ])

    todaycourseSessions.value = courseSessionsData || []

    if (reportData) {
      stats.value = {
        activeCourseSessions: reportData.totalCourseSessions || 0,
        pendingConfirmations: todaycourseSessions.value.filter(c => c.status?.toLowerCase() === 'active').length,
        averageAttendance: reportData.overallAttendance || 0
      }
    }
  } catch (err) {
    console.error('Failed to load dashboard data:', err)
  } finally {
    isLoading.value = false
  }
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
      // Flatten daily records into attendance records
      recentRecords.value = reportData.dailyRecords.map(day => ({
        checkInTime: day.date,
        isConfirmed: true,
        courseName: `${day.totalCourseSessions} courseSessions`,
        professor: `${day.attendanceRate}% attendance`
      }))
    }
  } catch (err) {
    console.error('Failed to load recent records:', err)
  } finally {
    isLoadingRecords.value = false
  }
}

async function confirmAttendance(courseSessionId: number) {
  try {
    await attendanceStore.confirmAttendance(courseSessionId)
    await loadDashboardData()
  } catch (err) {
    console.error('Failed to confirm attendance:', err)
  }
}

function viewAttendance(courseSessionId: number) {
  router.push(`/dashboard/attendance/${courseSessionId}`)
}

function OpenCourseSessionSession() {
  router.push('/dashboard/attendance/open-session')
}

onMounted(() => {
  loadDashboardData()
  loadRecentRecords()
})
</script>