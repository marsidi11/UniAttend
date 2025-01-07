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
          <option v-for="group in groups" :key="group.id" :value="group.id">
            {{ group.name }}
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
import { useGroupStore } from '@/stores/group.store'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'

const attendanceStore = useAttendanceStore()
const groupStore = useGroupStore()

const { records, isLoading } = storeToRefs(attendanceStore)
const { groups } = storeToRefs(groupStore)

const selectedGroup = ref('')

const columns = [
  { key: 'studentName', label: 'Student' },
  { key: 'groupName', label: 'Group' },
  { key: 'checkInTime', label: 'Check-in Time',
    render: (value: string) => new Date(value).toLocaleString()
  },
  { key: 'checkInMethod', label: 'Method' },
  { key: 'isConfirmed', label: 'Status',
    render: (value: boolean) => value ? 'Confirmed' : 'Pending'
  }
]

const filteredRecords = computed(() => {
  let filtered = [...records.value]
  if (selectedGroup.value) {
    filtered = filtered.filter(r => r.groupId === Number(selectedGroup.value))
  }
  return filtered
})

async function loadData() {
  try {
    await Promise.all([
      attendanceStore.fetchAttendance(),
      groupStore.fetchGroups()
    ])
  } catch (err) {
    console.error('Failed to load data:', err)
  }
}

async function exportAttendance() {
  try {
    await attendanceStore.generateAttendanceList(Number(selectedGroup.value))
  } catch (err) {
    console.error('Failed to export attendance:', err)
  }
}

onMounted(() => {
  loadData()
})
</script>