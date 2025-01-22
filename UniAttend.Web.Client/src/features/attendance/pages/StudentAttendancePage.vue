<template>
  <div class="space-y-6">
    <header class="flex justify-between items-center">
      <h1 class="text-2xl font-bold">My Attendance</h1>
    </header>

    <div class="grid grid-cols-3 gap-4">
      <StatCard
        title="Total Classes"
        :value="stats.totalClasses || 0"
      />
      <StatCard
        title="Classes Attended"
        :value="stats.attendedClasses || 0"
      />
      <StatCard
        title="Attendance Rate"
        :value="`${stats.attendanceRate || 0}%`"
      />
    </div>

    <AttendanceList
      :records="attendanceRecords"
      :loading="isLoading"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useAttendanceStore } from '@/stores/attendance.store'
import { useReportStore } from '@/stores/report.store'
import StatCard from '@/shared/components/ui/StatCard.vue'
import AttendanceList from '../components/AttendanceList.vue'
import type { 
  AttendanceRecordDto,
  AttendanceStatsDto 
} from '@/api/generated/data-contracts'

// Store setup
const attendanceStore = useAttendanceStore()
const reportStore = useReportStore()

// Store refs
const { isLoading } = storeToRefs(attendanceStore)

// Component state
const attendanceRecords = ref<AttendanceRecordDto[]>([])
const stats = ref<AttendanceStatsDto>({
  totalClasses: 0,
  attendedClasses: 0,
  attendanceRate: 0
})

async function loadAttendance() {
  try {
    // Fetch attendance records
    const attendanceData = await attendanceStore.fetchAttendance()
    attendanceRecords.value = attendanceData

    // Get stats from student report
    const reportData = await reportStore.getMyReport()
    if (reportData) {
      stats.value = {
        totalClasses: reportData.totalClasses || 0,
        attendedClasses: reportData.totalAttendance || 0,
        attendanceRate: reportData.attendanceRate || 0
      }
    }
  } catch (err) {
    console.error('Failed to load attendance:', err)
  }
}

onMounted(() => {
  loadAttendance()
})
</script>