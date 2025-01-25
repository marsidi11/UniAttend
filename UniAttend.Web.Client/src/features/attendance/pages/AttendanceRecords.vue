<template>
  <div class="space-y-6">
    <header class="flex justify-between items-center">
      <h1 class="text-2xl font-bold">Attendance Records</h1>
      <div class="flex gap-4">
        <select
          v-model="selectedGroup"
          class="rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All Groups</option>
          <option v-for="studyGroup in studyGroups" :key="studyGroup.id" :value="studyGroup.id">
            {{ studyGroup.name }}
          </option>
        </select>
        <Button @click="exportAttendance">Export</Button>
      </div>
    </header>

    <div class="bg-white shadow rounded-lg">
      <DataTable
        :data="filteredRecords"
        :columns="columns"
        :loading="isLoading"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useAttendanceStore } from '@/stores/attendance.store'
import { useGroupStore } from '@/stores/studyGroup.store'
import { useReportStore } from '@/stores/report.store'
import type { AttendanceRecordDto } from '@/api/generated/data-contracts'
import type { TableItem } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'

// Store setup
const attendanceStore = useAttendanceStore()
const groupStore = useGroupStore()
const reportStore = useReportStore()

// Store refs
const { records, isLoading } = storeToRefs(attendanceStore)
const { studyGroups } = storeToRefs(groupStore)

// Component state
const selectedGroup = ref('')

// Table columns
const columns = [
  { key: 'studentName', label: 'Student' },
  { key: 'studyGroupName', label: 'Group' },
  { key: 'checkInTime', label: 'Check-in Time',
    render: (value: string) => new Date(value).toLocaleString()
  },
  { key: 'checkInMethod', label: 'Method' },
  { key: 'isConfirmed', label: 'Status',
    render: (value: boolean) => value ? 'Confirmed' : 'Pending'
  }
]

// Computed properties
const filteredRecords = computed(() => {
  let filtered = records.value.map((record, index) => ({
    ...record,
    id: index, // Add required id for TableItem
    studentName: record.courseName, // Map courseName to studentName
    studyGroupId: undefined // Add studyGroupId property
  })) as (AttendanceRecordDto & TableItem)[]

  if (selectedGroup.value) {
    filtered = filtered.filter(r => r.studyGroupId === Number(selectedGroup.value))
  }

  return filtered
})

// Methods
async function loadData() {
  try {
    await Promise.all([
      attendanceStore.fetchAttendance(),
      groupStore.fetchStudyGroups()
    ])
  } catch (err) {
    console.error('Failed to load data:', err)
  }
}

async function exportAttendance() {
  try {
    if (selectedGroup.value) {
      await reportStore.exportAttendanceReport(
        Number(selectedGroup.value),
        new Date(), // Start date
        new Date() // End date
      )
    }
  } catch (err) {
    console.error('Failed to export attendance:', err)
  }
}

// Lifecycle hooks
onMounted(() => {
  loadData()
})
</script>