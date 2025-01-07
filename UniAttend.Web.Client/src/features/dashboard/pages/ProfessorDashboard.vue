<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Professor Dashboard</h1>
      <Button @click="openClassSession" variant="primary">
        Open Class Session
      </Button>
    </div>

    <!-- Stats Overview -->
    <div class="grid grid-cols-3 gap-4">
      <StatCard title="Active Classes" :value="stats.activeClasses" subtitle="Today's Sessions" />
      <StatCard title="Pending Confirmations" :value="stats.pendingConfirmations"
        :status="stats.pendingConfirmations > 0 ? 'warning' : 'success'" />
      <StatCard title="Attendance Rate" :value="`${stats.averageAttendance}%`" subtitle="Overall Average" />
    </div>

    <!-- Content Grid -->
    <div class="grid grid-cols-2 gap-4">
      <!-- Today's Classes -->
      <div class="bg-white p-6 rounded-lg shadow">
        <h2 class="text-lg font-medium mb-4">Today's Classes</h2>
        <div class="space-y-4">
          <div v-if="isLoading" class="flex justify-center">
            <Spinner :size="6" />
          </div>
          <div v-else-if="!todayClasses.length" class="text-gray-500 text-center">
            No classes scheduled for today
          </div>
          <div v-else v-for="class_ in todayClasses" :key="class_.id"
            class="flex justify-between items-center p-4 bg-gray-50 rounded-md">
            <div>
              <p class="font-medium">{{ class_.groupName }}</p>
              <p class="text-sm text-gray-500">
                {{ formatSessionTime(class_.startTime) }} - {{ class_.classroom }}
              </p>
              <Badge :status="getStatusBadgeType(class_.status)">
                {{ class_.status }}
              </Badge>
            </div>
            <div class="flex space-x-2">
              <Button v-if="class_.status === 'active'" @click="confirmAttendance(class_.id)" variant="primary">
                Confirm Attendance
              </Button>
              <Button @click="viewAttendance(class_.id)" variant="secondary">
                View Details
              </Button>
            </div>
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
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAttendanceStore } from '@/stores/attendance.store'
import type { AttendanceRecord } from '@/types/attendance.types'
import { formatDate } from '@/utils/dateUtils'
import Button from '@/shared/components/ui/Button.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import AttendanceList from '@/features/attendance/components/AttendanceList.vue'

type BadgeStatus = 'success' | 'warning' | 'error' | 'info'

// UI-specific interface
interface ClassSession {
  id: number
  groupId: number
  classroomId: number
  date: Date
  startTime: string
  endTime: string
  groupName: string
  classroom: string
  status: 'active' | 'pending' | 'completed'
}

interface DashboardStats {
  activeClasses: number
  pendingConfirmations: number
  averageAttendance: number
}

// Template helper functions
function formatSessionTime(time: string) {
  return formatDate(new Date(time))
}

// Rest of the component code...
const router = useRouter()
const attendanceStore = useAttendanceStore()

const isLoading = ref(false)
const isLoadingRecords = ref(false)
const stats = ref<DashboardStats>({
  activeClasses: 0,
  pendingConfirmations: 0,
  averageAttendance: 0
})
const todayClasses = ref<ClassSession[]>([])
const recentRecords = ref<AttendanceRecord[]>([])

function getStatusBadgeType(status: ClassSession['status']): BadgeStatus {
  switch (status) {
    case 'active': return 'success'
    case 'pending': return 'warning'
    default: return 'info'
  }
}

async function loadDashboardData() {
  isLoading.value = true
  try {
    const [todaySessionsData, statsData] = await Promise.all([
      attendanceStore.fetchTodaySessions(),
      attendanceStore.getAttendanceStats()
    ])
    
    // Map API data to UI model ensuring all required fields
    todayClasses.value = todaySessionsData.map(session => ({
      id: session.id,
      groupId: session.groupId,
      classroomId: session.classroomId,
      date: new Date(session.date),
      startTime: session.startTime,
      endTime: session.endTime,
      groupName: session.groupName || '',
      classroom: session.classroom || '',
      status: session.status.toLowerCase() as ClassSession['status']
    }))

    stats.value = {
      activeClasses: statsData.totalClasses || 0,
      pendingConfirmations: statsData.pendingConfirmations || 0,
      averageAttendance: statsData.attendanceRate || 0
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
    recentRecords.value = await attendanceStore.fetchRecentRecords()
  } catch (err) {
    console.error('Failed to load recent records:', err)
  } finally {
    isLoadingRecords.value = false
  }
}

async function confirmAttendance(classId: number) {
  try {
    await attendanceStore.confirmAttendance(classId)
    await loadDashboardData()
  } catch (err) {
    console.error('Failed to confirm attendance:', err)
  }
}

function viewAttendance(classId: number) {
  router.push(`/dashboard/attendance/${classId}`)
}

function openClassSession() {
  router.push('/dashboard/attendance/open-session')
}

onMounted(() => {
  loadDashboardData()
  loadRecentRecords()
})
</script>