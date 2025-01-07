<template>
  <div class="space-y-2">
    <div
      v-for="schedule in schedules"
      :key="schedule.id"
      class="p-2 rounded text-sm cursor-pointer"
      :class="[
        'hover:bg-opacity-75',
        {
          'bg-blue-100 text-blue-800': schedule.type === 'lecture',
          'bg-green-100 text-green-800': schedule.type === 'practice',
          'bg-purple-100 text-purple-800': schedule.type === 'lab'
        }
      ]"
      @click="$emit('click', schedule)"
    >
      <div class="font-medium">{{ schedule.groupName }}</div>
      <div class="text-xs">{{ schedule.subjectName }}</div>
      <div class="text-xs text-gray-500">
        {{ schedule.classroomName }}
      </div>
      <div class="text-xs text-gray-500">
        {{ schedule.professorName }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Schedule } from '@/types/schedule.types'

interface Props {
  schedules: Schedule[]
}

defineProps<Props>()

defineEmits<{
  (e: 'click', schedule: Schedule): void
}>()
</script>

<style scoped>
.schedule-slot {
  min-height: 5rem;
}

/* Optional: Add transition for hover effects */
.schedule-slot-item {
  transition: all 0.2s ease-in-out;
}
</style>