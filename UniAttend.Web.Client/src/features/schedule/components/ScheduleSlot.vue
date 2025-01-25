<template>
  <div class="schedule-container" :class="{'has-multiple': schedules.length > 1}">
    <TransitionGroup name="schedule-list" tag="div" class="schedule-stack">
      <div v-for="(schedule, index) in schedules"
           :key="schedule.id ?? index"
           class="schedule-card"
           :style="{ zIndex: schedules.length - index }"
           :class="[
             getCardcourseSessions(schedule),
             {'is-expanded': expandedId === (schedule.id ?? -1)},
             {'stacked-card': schedules.length > 1 && index > 0}
           ]"
           @click="toggleExpand(schedule.id ?? -1)">
        
        <!-- Compact View -->
        <div class="card-header">
          <div class="flex justify-between items-start w-full">
            <div class="flex flex-col min-w-0 flex-1">
              <div class="flex items-center gap-2">
                <span class="font-medium text-sm truncate">{{ schedule.studyGroupName }}</span>
                <span class="text-xs bg-white bg-opacity-50 px-2 py-0.5 rounded-full">
                  {{ schedule.classroomName }}
                </span>
              </div>
              <span class="text-xs text-gray-600 truncate">{{ schedule.subjectName }}</span>
            </div>
          </div>
        </div>

        <!-- Expanded View -->
        <div v-if="expandedId === (schedule.id ?? -1)" 
             class="expanded-content"
             @click.stop>
          <div class="grid grid-cols-2 gap-3 text-sm mt-2 pt-2 border-t border-gray-200">
            <div>
              <span class="text-gray-500 text-xs">Professor</span>
              <p class="font-medium">{{ schedule.professorName }}</p>
            </div>
            <div>
              <span class="text-gray-500 text-xs">Time</span>
              <p class="font-medium">
                {{ formatTimeString(schedule.startTime) }} - {{ formatTimeString(schedule.endTime) }}
              </p>
            </div>
          </div>
          
          <div class="action-buttons">
            <button @click.stop="$emit('click', schedule)" 
                    class="edit-button">
              Edit
            </button>
            <button v-if="isFirstSlot(schedule)"
                    @click.stop="$emit('delete', schedule)" 
                    class="delete-button">
              Delete
            </button>
          </div>
        </div>
      </div>
    </TransitionGroup>
    
    <!-- Schedule Count Indicator -->
    <div v-if="schedules.length > 1" 
         class="schedule-count">
      {{ schedules.length }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { ScheduleDto } from '@/api/generated/data-contracts'

interface TimeObject {
  hours: number;
  minutes: number;
}

const props = defineProps<{
  schedules: ScheduleDto[]
  currentTime: string
}>()

defineEmits<{
  (e: 'click', schedule: ScheduleDto): void
  (e: 'delete', schedule: ScheduleDto): void
}>()

const expandedId = ref<number | null>(null)

function toggleExpand(id: number) {
  expandedId.value = expandedId.value === id ? null : id
}

function getCardcourseSessions(schedule: ScheduleDto) {
  const baseClass = 'bg-gradient-to-br'
  const colorcourseSessions = [
    'from-blue-50 to-blue-100 border-blue-200',
    'from-emerald-50 to-emerald-100 border-emerald-200',
    'from-violet-50 to-violet-100 border-violet-200',
    'from-amber-50 to-amber-100 border-amber-200',
    'from-rose-50 to-rose-100 border-rose-200'
  ]
  const index = (schedule.studyGroupId ?? 0) % colorcourseSessions.length
  return `${baseClass} ${colorcourseSessions[index]}`
}

function formatTimeString(time: string | TimeObject | unknown): string {
  if (typeof time === 'string') {
    return time.substring(0, 5)
  }
  if (time && typeof time === 'object' && 'hours' in time && 'minutes' in time) {
    const timeObj = time as TimeObject
    const hours = String(timeObj.hours).padStart(2, '0')
    const minutes = String(timeObj.minutes).padStart(2, '0')
    return `${hours}:${minutes}`
  }
  return '00:00'
}

function isFirstSlot(schedule: ScheduleDto): boolean {
  const scheduleStart = timeToMinutes(formatTimeString(schedule.startTime))
  const currentSlot = timeToMinutes(props.currentTime)
  return scheduleStart === currentSlot
}

function timeToMinutes(time: string): number {
  const [hours, minutes] = time.split(':').map(Number)
  return (hours || 0) * 60 + (minutes || 0)
}
</script>

<style scoped>
.schedule-container {
  @apply h-full w-full overflow-visible relative;
}

.schedule-stack {
  @apply relative space-y-1;
}

.schedule-card {
  @apply relative border rounded-lg p-2.5 cursor-pointer
         transition-all duration-200 shadow-sm hover:shadow-md
         bg-white;
  min-height: 4.5rem;
}

.stacked-card {
  @apply -mt-3 hover:-translate-y-1;
}

.has-multiple .schedule-card {
  @apply hover:z-50;
}

.is-expanded {
  @apply shadow-lg ring-1 ring-black ring-opacity-5 z-50
         scale-105 bg-white;
}

.expanded-content {
  @apply animate-fadeIn bg-white relative;
}

.action-buttons {
  @apply flex justify-end gap-2 mt-3 pt-2 border-t;
}

.edit-button {
  @apply px-3 py-1 text-xs font-medium text-indigo-600 
         hover:text-indigo-800 rounded-md hover:bg-indigo-50
         transition-colors duration-200;
}

.delete-button {
  @apply px-3 py-1 text-xs font-medium text-red-600
         hover:text-red-800 rounded-md hover:bg-red-50
         transition-colors duration-200;
}

.schedule-count {
  @apply absolute -top-1 -right-1 w-5 h-5 rounded-full
         bg-indigo-500 text-white text-xs font-medium
         flex items-center justify-center shadow-sm z-50;
}

/* Animation for expanding cards */
.animate-fadeIn {
  animation: fadeIn 0.2s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(-0.25rem); }
  to { opacity: 1; transform: translateY(0); }
}

@media (max-width: 640px) {
  .schedule-card {
    @apply p-2;
    min-height: 4rem;
  }
}
</style>