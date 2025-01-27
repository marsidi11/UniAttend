<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Student Dashboard</h1>
      <div class="space-x-2">
        <Button @click="router.push('/dashboard/attendance/otp-check-in')" variant="secondary">
          OTP Check-in
        </Button>
        <Button @click="router.push('/dashboard/attendance/check-in')">
          Card Check-in
        </Button>
      </div>
    </div>

    <!-- Personal Stats -->
    <div class="grid grid-cols-3 gap-4">
      <StatCard
        title="Total Course Sessions"
        :value="stats.totalCourseSessions"
      />
      <StatCard
        title="courseSessions Attended"
        :value="stats.attendedCourseSessions"
      />
      <StatCard
        title="Absence Rate"
        :value="`${absenceRate}%`"
        :status="getAbsenceStatus(absenceRate)"
      />
    </div>

    <!-- Content Grid -->
    <div class="grid grid-cols-2 gap-4">
      <!-- Today's Schedule -->
      <div class="bg-white p-6 rounded-lg shadow">
        <h2 class="text-lg font-medium mb-4">Today's Schedule</h2>
        <div class="space-y-4">
          <div v-if="isLoadingSchedule" class="flex justify-center">
            <Spinner :size="6" />
          </div>
          <div v-else-if="!todaycourseSessions.length" class="text-gray-500 text-center">
            No Course Sessions scheduled for today
          </div>
          <div 
            v-else
            v-for="class_ in todaycourseSessions" 
            :key="class_.id"
            class="flex justify-between items-center p-4 bg-gray-50 rounded-md"
          >
            <div>
              <p class="font-medium">{{ class_.subjectName }}</p>
              <p class="text-sm text-gray-500">
                {{ formatTime(class_.startTime) }} - {{ class_.classroom }}
              </p>
              <p v-if="class_.absenceAlert" class="text-sm text-red-600">
                High absence rate warning!
              </p>
            </div>
            <Badge :status="class_.attended ? 'success' : 'warning'">
              {{ class_.attended ? 'Attended' : 'Not Attended' }}
            </Badge>
          </div>
        </div>
      </div>

      <!-- Recent Attendance -->
      <div class="bg-white p-6 rounded-lg shadow">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-lg font-medium">Recent Attendance</h2>
          <Button 
            variant="secondary" 
            size="sm"
            @click="router.push('/dashboard/attendance/view')"
          >
            View All
          </Button>
        </div>
        <div class="space-y-4">
          <div v-if="isLoadingAttendance" class="flex justify-center">
            <Spinner :size="6" />
          </div>
          <div v-else>
            <AttendanceList
              :records="recentAttendance"
              :loading="false"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAttendanceStore } from '@/stores/attendance.store'
import { formatTime } from '@/utils/dateUtils'
import Button from '@/shared/components/ui/Button.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import AttendanceList from '@/features/attendance/components/AttendanceList.vue'
import type { ClassSession, AttendanceRecord, AttendanceStats } from '@/types/attendance.types'

const router = useRouter()
const attendanceStore = useAttendanceStore()

const isLoadingSchedule = ref(false)
const isLoadingAttendance = ref(false)
const stats = ref<AttendanceStats>({
  totalCourseSessions: 0,
  attendedCourseSessions: 0,
  attendanceRate: 0,
  pendingConfirmations: 0
})
const todaycourseSessions = ref<ClassSession[]>([])
const recentAttendance = ref<AttendanceRecord[]>([])

const absenceRate = computed(() => {
  if (!stats.value.totalCourseSessions) return 0
  return Math.round(((stats.value.totalCourseSessions - stats.value.attendedCourseSessions) / stats.value.totalCourseSessions) * 100)
})

function getAbsenceStatus(rate: number): 'success' | 'warning' | 'error' {
  if (rate <= 20) return 'success'
  if (rate <= 30) return 'warning'
  return 'error'
}

async function loadDashboardData() {
  try {
    const data = await attendanceStore.getAttendanceStats()
    stats.value = data
  } catch (err) {
    console.error('Failed to load dashboard stats:', err)
  }
}

async function loadTodaySchedule() {
  isLoadingSchedule.value = true
  try {
    const schedule = await attendanceStore.fetchTodaySessions()
    todaycourseSessions.value = schedule.map(session => ({
      ...session,
      absenceAlert: (session.absenceRate ?? 0) > 20
    }))
  } catch (err) {
    console.error('Failed to load schedule:', err)
  } finally {
    isLoadingSchedule.value = false
  }
}

async function loadRecentAttendance() {
  isLoadingAttendance.value = true
  try {
    const records = await attendanceStore.fetchRecentRecords()
    recentAttendance.value = records
  } catch (err) {
    console.error('Failed to load attendance:', err)
  } finally {
    isLoadingAttendance.value = false
  }
}

onMounted(() => {
  loadDashboardData()
  loadTodaySchedule()
  loadRecentAttendance()
})
</script>