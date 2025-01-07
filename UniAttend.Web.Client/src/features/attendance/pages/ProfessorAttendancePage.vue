<template>
  <div class="space-y-6">
    <header class="flex justify-between items-center">
      <h1 class="text-2xl font-bold">Class Attendance</h1>
      <Button 
        variant="primary" 
        @click="confirmAttendance"
        :disabled="!hasUnconfirmedRecords"
      >
        Confirm Attendance
      </Button>
    </header>

    <div class="grid grid-cols-3 gap-4">
      <StatCard
        title="Total Students"
        :value="stats.totalStudents"
      />
      <StatCard
        title="Present Today"
        :value="stats.presentToday"
      />
      <StatCard
        title="Attendance Rate"
        :value="`${stats.attendanceRate}%`"
      />
    </div>

    <AttendanceList 
      v-model:selected="selectedRecords"
      :records="currentClassRecords" 
      selectable
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAttendanceStore } from '@/stores/attendance.store'
import { useRoute } from 'vue-router'
import Button from '@/shared/components/ui/Button.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import AttendanceList from '../components/AttendanceList.vue'
import type { AttendanceRecord, ClassAttendance, AttendanceStats } from '@/types/attendance.types'

type ClassStats = AttendanceStats

const route = useRoute()
const attendanceStore = useAttendanceStore()
const selectedRecords = ref<number[]>([])
const currentClassRecords = ref<AttendanceRecord[]>([])
const stats = ref<ClassStats>({
  totalStudents: 0,
  presentToday: 0,
  attendanceRate: 0
})

const classId = computed(() => Number(route.params.id))
const hasUnconfirmedRecords = computed(() => 
  currentClassRecords.value.some(record => !record.isConfirmed)
)

async function loadClassAttendance() {
  try {
    const response = await attendanceStore.fetchClassAttendance(classId.value)
    if (response) {
      currentClassRecords.value = response.records
      stats.value = response.stats
    }
  } catch (error) {
    console.error('Failed to load class attendance:', error)
  }
}

async function confirmAttendance() {
  try {
    await attendanceStore.confirmAttendance(classId.value)
    await loadClassAttendance()
  } catch (error) {
    console.error('Failed to confirm attendance:', error)
  }
}

onMounted(() => {
  loadClassAttendance()
})
</script>