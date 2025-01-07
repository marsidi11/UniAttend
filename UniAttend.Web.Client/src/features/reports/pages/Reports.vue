<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Reports</h1>
    </div>

    <!-- Stats Overview -->
    <div class="grid grid-cols-3 gap-4">
      <StatCard 
        title="Total Alerts" 
        :value="summary?.totalAlerts || 0"
      />
      <StatCard 
        title="Unsent Alerts" 
        :value="summary?.unsentAlerts || 0"
        :change="alertChangePercentage"
      />
      <StatCard 
        title="Critical Cases" 
        :value="summary?.criticalAlerts || 0"
      />
    </div>

    <!-- Quick Links -->
    <div class="grid grid-cols-2 gap-4">
      <router-link 
        to="/dashboard/reports/absence-alerts"
        class="bg-white p-6 rounded-lg shadow hover:shadow-md transition-shadow"
      >
        <h3 class="text-lg font-medium mb-2">Absence Alerts</h3>
        <p class="text-gray-600">View and manage student absence alerts</p>
      </router-link>

      <div class="bg-white p-6 rounded-lg shadow">
        <h3 class="text-lg font-medium mb-2">Attendance Reports</h3>
        <p class="text-gray-600">Download detailed attendance reports</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useAbsenceAlertStore } from '@/stores/absenceAlert.store'
import StatCard from '@/shared/components/ui/StatCard.vue'
import type { AlertSummary } from '@/types/alert.types'

const alertStore = useAbsenceAlertStore()
const summary = ref<AlertSummary | null>(null)
const previousSummary = ref<AlertSummary | null>(null)

const alertChangePercentage = computed(() => {
  if (!summary.value?.unsentAlerts || !previousSummary.value?.unsentAlerts) return 0
  const change = ((summary.value.unsentAlerts - previousSummary.value.unsentAlerts) 
    / previousSummary.value.unsentAlerts) * 100
  return Math.round(change)
})

onMounted(async () => {
  try {
    summary.value = await alertStore.fetchAlertSummary()
  } catch (err) {
    console.error('Failed to load alert summary:', err)
  }
})
</script>