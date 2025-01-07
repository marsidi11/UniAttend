<template>
  <div class="space-y-6">
    <header class="flex justify-between items-center">
      <h1 class="text-2xl font-bold">My Attendance</h1>
    </header>

    <div class="grid grid-cols-3 gap-4">
      <StatCard
        title="Total Classes"
        :value="stats.totalClasses"
      />
      <StatCard
        title="Classes Attended"
        :value="stats.attendedClasses"
      />
      <StatCard
        title="Attendance Rate"
        :value="`${stats.attendanceRate}%`"
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
import StatCard from '@/shared/components/ui/StatCard.vue'
import AttendanceList from '../components/AttendanceList.vue'
import type { AttendanceRecord } from '@/types/attendance.types'

const attendanceStore = useAttendanceStore()
const { isLoading } = storeToRefs(attendanceStore)

const attendanceRecords = ref<AttendanceRecord[]>([])
const stats = ref({
  totalClasses: 0,
  attendedClasses: 0,
  attendanceRate: 0
})

async function loadAttendance() {
  try {
    await attendanceStore.fetchAttendance()
    const data = await attendanceStore.getAttendanceStats()
    stats.value = data
    attendanceRecords.value = attendanceStore.records
  } catch (err) {
    console.error('Failed to load attendance:', err)
  }
}

onMounted(() => {
  loadAttendance()
})
</script>