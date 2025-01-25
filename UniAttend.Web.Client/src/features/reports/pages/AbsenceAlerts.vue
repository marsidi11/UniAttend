<template>
    <div class="space-y-6">
      <!-- Header -->
      <div class="flex justify-between items-center">
        <h1 class="text-2xl font-bold text-gray-900">Absence Alerts</h1>
        <div class="flex gap-4">
          <Button 
            variant="secondary" 
            @click="exportAlerts"
            :disabled="!filteredAlerts.length"
          >
            Export
          </Button>
          <Button 
            @click="sendSelectedAlerts"
            :disabled="!selectedAlerts.length"
          >
            Send Selected Alerts
          </Button>
        </div>
      </div>
  
      <!-- Filters -->
      <div class="flex gap-4 bg-white p-4 rounded-lg shadow">
        <div class="w-64">
          <label class="block text-sm font-medium text-gray-700">Status</label>
          <select
            v-model="selectedStatus"
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
          >
            <option value="">All</option>
            <option value="unsent">Unsent</option>
            <option value="sent">Sent</option>
            <option value="critical">Critical</option>
          </select>
        </div>
      </div>
  
      <!-- Alerts Table -->
      <div class="bg-white shadow rounded-lg">
        <DataTable
          :data="filteredAlerts"
          :columns="columns"
          :loading="alertStore.isLoading"
          :selectable="true"
          v-model:selected="selectedAlerts"
        />
      </div>
    </div>
  </template>
  
  <!-- <script setup lang="ts">
  import { ref, computed, onMounted } from 'vue'
  import { useAbsenceAlertStore } from '@/stores/absenceAlert.store'
  import Button from '@/shared/components/ui/Button.vue'
  import DataTable from '@/shared/components/ui/DataTable.vue'
  import type { AbsenceAlert } from '@/types/alert.types'
  import type { Column, TableItem } from '@/types/tableItem.types'
  
  const alertStore = useAbsenceAlertStore()
  const selectedStatus = ref('')
  const selectedAlerts = ref<number[]>([])
  
  interface ExtendedAlert extends AbsenceAlert, TableItem {}
  
  const columns: Column<TableItem>[] = [
    { 
      key: 'student',
      label: 'Student',
      render: (value: any) => {
        const student = value as ExtendedAlert['student']
        return student ? `${student.firstName} ${student.lastName}` : ''
      }
    },
    { 
      key: 'group',
      label: 'Group',
      render: (value: any) => {
        const group = value as ExtendedAlert['group']
        return studyGroup?.name ?? ''
      }
    },
    { 
      key: 'subjectName',
      label: 'Subject',
      render: (value: any) => String(value)
    },
    { 
      key: 'absencePercentage',
      label: 'Absence Rate',
      render: (value: any) => `${Number(value)}%`
    },
    { 
      key: 'emailSent',
      label: 'Status',
      render: (value: any) => Boolean(value) ? 'Sent' : 'Pending'
    },
    { 
      key: 'createdAt',
      label: 'Created',
      render: (value: any) => new Date(value).toLocaleDateString()
    }
  ]
  
  const filteredAlerts = computed(() => {
    let filtered = alertStore.alerts as unknown as TableItem[]
    
    if (selectedStatus.value) {
      switch (selectedStatus.value) {
        case 'unsent':
          filtered = alertStore.unsentAlerts as unknown as TableItem[]
          break
        case 'sent':
          filtered = filtered.filter(alert => (alert as ExtendedAlert).emailSent)
          break
        case 'critical':
          filtered = alertStore.criticalAlerts as unknown as TableItem[]
          break
      }
    }
    
    return filtered
  })

async function sendSelectedAlerts() {
  try {
    await Promise.all(
      selectedAlerts.value.map(alertId => alertStore.markAsSent(alertId))
    )
    selectedAlerts.value = []
  } catch (err) {
    console.error('Failed to send alerts:', err)
  }
}

function exportAlerts() {
  // Implement export functionality
  console.log('Exporting alerts...')
}

onMounted(async () => {
  try {
    // Pass null instead of undefined to fetch all alerts
    await alertStore.fetchStudentAlerts(null as unknown as number)
  } catch (err) {
    console.error('Failed to load alerts:', err)
  }
})
</script> -->