<template>
  <div class="space-y-2">
    <div v-for="schedule in schedules" 
         :key="schedule.id"
         class="group p-2 rounded text-sm cursor-pointer relative"
         :class="['hover:bg-opacity-75', getScheduleClass(schedule)]">
      <!-- Delete button -->
      <button 
        @click.stop="$emit('delete', schedule)"
        class="absolute top-1 right-1 opacity-0 group-hover:opacity-100 
               text-red-600 hover:text-red-800 transition-opacity">
        <span class="material-icons text-sm">delete</span>
      </button>
      
      <!-- Existing schedule content -->
      <div @click="$emit('click', schedule)">
        <div class="font-medium">{{ schedule.groupName }}</div>
        <div class="text-xs">{{ schedule.subjectName }}</div>
        <div class="text-xs text-gray-500">{{ schedule.classroomName }}</div>
        <div class="text-xs text-gray-500">{{ schedule.professorName }}</div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ScheduleDto } from '@/api/generated/data-contracts' 

interface Props {
  schedules: ScheduleDto[]
}

defineProps<Props>()

defineEmits<{
  (e: 'click', schedule: ScheduleDto): void
  (e: 'delete', schedule: ScheduleDto): void
}>()

function getScheduleClass(schedule: ScheduleDto) {
  // You can customize this logic based on your schedule properties
  // For example, based on subject name or group name
  return {
    'bg-blue-100 text-blue-800': true // Default to lecture style
    // Add more conditions if needed:
    // 'bg-green-100 text-green-800': isLabSchedule(schedule),
    // 'bg-purple-100 text-purple-800': isPracticeSchedule(schedule)
  }
}
</script>

<style scoped>
.schedule-slot {
  min-height: 5rem;
}

.schedule-slot-item {
  transition: all 0.2s ease-in-out;
}
</style>