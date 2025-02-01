<template>
  <div class="schedule-container">
    <div v-for="(schedule, index) in schedules" 
         :key="schedule.id ?? index" 
         class="schedule-card"
         :class="[
           getCardcourseSessions(schedule),
           { 'expanded': expandedId === (schedule.id ?? -1) }
         ]"
         @click="toggleExpand(schedule.id ?? -1)">
      
      <!-- Basic Info -->
      <div class="basic-info">
        <div class="header">
          <span class="title">{{ schedule.studyGroupName }}</span>
          <span class="time">{{ formatTimeString(schedule.startTime) }}</span>
        </div>
        <div class="details">
          <span class="classroom">{{ schedule.classroomName }}</span>
          <span class="subject">{{ schedule.subjectName }}</span>
        </div>
      </div>

      <!-- Expanded Info -->
      <div v-if="expandedId === (schedule.id ?? -1)" class="expanded-info">
        <div class="expanded-grid">
          <div class="info-item">
            <span class="label">Professor</span>
            <span class="value">{{ schedule.professorName }}</span>
          </div>
          <div class="info-item">
            <span class="label">Time</span>
            <span class="value">{{ formatTimeString(schedule.startTime) }} - {{ formatTimeString(schedule.endTime) }}</span>
          </div>
        </div>
        
        <div v-if="showActions" class="actions">
          <button class="action-btn edit" @click.stop="$emit('click', schedule)">
            Edit
          </button>
          <button v-if="isFirstSlot(schedule)" 
                  class="action-btn delete" 
                  @click.stop="$emit('delete', schedule)">
            Delete
          </button>
        </div>
      </div>
    </div>

    <div v-if="schedules.length > 1" class="counter">
      {{ schedules.length }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import { useAuthStore } from '@/stores/auth.store'
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

const showActions = computed(() => {
  const authStore = useAuthStore()
  return ['admin', 'secretary'].includes(authStore.userRole || '')
})

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
    return time.split('.')[0].substring(0, 5)
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
  return (hours * 60) + minutes
}
</script>

<style scoped>
.schedule-container {
  @apply relative h-full w-full;
}

.schedule-card {
  @apply mb-2 p-3 rounded-lg border cursor-pointer transition-all duration-200;
  background-color: white;
}

.schedule-card:hover {
  @apply shadow-md transform -translate-y-0.5;
}

.schedule-card.expanded {
  @apply shadow-lg z-50 relative;
}

.basic-info {
  @apply space-y-1;
}

.header {
  @apply flex justify-between items-start;
}

.title {
  @apply font-medium text-gray-900;
}

.time {
  @apply text-sm text-gray-600;
}

.details {
  @apply flex flex-col text-sm;
}

.classroom {
  @apply text-gray-600;
}

.subject {
  @apply text-gray-500;
}

.expanded-info {
  @apply mt-2 pt-2 border-t border-gray-100;
}

.expanded-grid {
  @apply grid grid-cols-1 gap-1;
}

.info-item {
  @apply flex flex-col items-start text-sm py-1;
}

.label {
  @apply text-gray-500 font-medium;
}

.value {
  @apply text-gray-700 mt-0.5;
}

.actions {
  @apply mt-3 pt-2 flex flex-col items-center gap-2 border-t border-gray-100;
}

.action-btn {
  @apply px-4 py-1.5 rounded text-sm font-medium w-full max-w-[120px];
}

.action-btn.edit {
  @apply bg-indigo-50 text-indigo-700 hover:bg-indigo-100;
}

.action-btn.delete {
  @apply bg-red-50 text-red-700 hover:bg-red-100;
}

.counter {
  @apply absolute top-0 right-0 -mt-2 -mr-2 w-5 h-5 
         flex items-center justify-center rounded-full 
         bg-indigo-500 text-white text-xs font-medium;
}
</style>