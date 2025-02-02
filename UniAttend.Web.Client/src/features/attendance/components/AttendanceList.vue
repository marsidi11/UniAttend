<template>
  <div>
    <div v-if="loading" class="flex justify-center py-4">
      <Spinner />
    </div>

    <div v-else-if="!records || records.length === 0" class="text-center py-4 text-gray-500">
      {{ emptyMessage || 'No attendance records found' }}
    </div>

    <table v-else class="min-w-full divide-y divide-gray-200">
      <thead class="bg-gray-50">
        <tr>
          <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Student</th>
          <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Check-in Time</th>
          <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Method</th>
          <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
          <th v-if="actions?.length"
            class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
            Actions
          </th>
        </tr>
      </thead>
      <tbody class="bg-white divide-y divide-gray-200">
        <tr v-for="record in records" :key="record.id">
          <td class="px-6 py-4 whitespace-nowrap">{{ record.studentName }}</td>
          <td class="px-6 py-4 whitespace-nowrap">{{ formatDateTime(record.checkInTime || '') }}</td>
          <td class="px-6 py-4 whitespace-nowrap">{{ formatCheckInMethod(record.checkInMethod || 0) }}</td>
          <td class="px-6 py-4 whitespace-nowrap">
            <Badge :status="record.isConfirmed ? 'success' : 'warning'">
              {{ record.isConfirmed ? 'Confirmed' : 'Pending' }}
            </Badge>
          </td>
          <!-- Add Actions Column -->
          <td v-if="actions?.length" class="px-6 py-4 whitespace-nowrap text-right text-sm">
            <Button v-for="action in actions" :key="action.label" v-show="!action.show || action.show(record)"
              @click="action.action(record)" variant="secondary" class="ml-2">
              <span v-if="action.icon" class="material-icons text-sm mr-1">{{ action.icon }}</span>
              {{ action.label }}
            </Button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { defineProps } from 'vue'
import type { AttendanceRecordDto, CheckInMethod } from '@/api/generated/data-contracts'
import Badge from '@/shared/components/ui/Badge.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import Button from '@/shared/components/ui/Button.vue'

defineProps<{
  records: AttendanceRecordDto[]
  loading?: boolean
  emptyMessage?: string
  compact?: boolean
  actions?: Array<{
    label: string
    icon?: string
    show?: (record: AttendanceRecordDto) => boolean
    action: (record: AttendanceRecordDto) => void
  }>
}>()

function formatDateTime(dateStr: string) {
  if (!dateStr) return ''
  const date = new Date(dateStr)
  return date.toLocaleString()
}

function formatCheckInMethod(method: CheckInMethod | number) {
  switch (method) {
    case 0: return 'Card'
    case 1: return 'OTP'
    default: return 'Unknown'
  }
}
</script>