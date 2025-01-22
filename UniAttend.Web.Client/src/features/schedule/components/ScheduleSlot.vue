<template>
  <div class="schedule-container">
    <TransitionGroup 
      name="schedule-list" 
      tag="div" 
      class="space-y-1">
      <div v-for="(schedule, index) in schedules"
           :key="schedule.id"
           class="schedule-card"
           :class="[
             getCardClasses(schedule),
             {'is-expanded': expandedId === schedule.id}
           ]"
           @click="toggleExpand(schedule.id)">
        
        <!-- Main Content -->
        <div class="card-content">
          <div class="primary-info">
            <div class="group-info">
              <span class="group-name">{{ schedule.groupName }}</span>
              <span class="subject-name">{{ schedule.subjectName }}</span>
            </div>
            <div class="secondary-info">
              <span class="room-badge">{{ schedule.classroomName }}</span>
              <span class="time-badge" v-if="isFirstSlot(schedule)">
                {{ formatTime(schedule.startTime) }}
              </span>
            </div>
          </div>

          <!-- Expanded Details -->
          <div v-if="expandedId === schedule.id" class="expanded-content">
            <div class="details-grid">
              <div class="detail-item">
                <span class="detail-label">Professor:</span>
                <span>{{ schedule.professorName }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">Duration:</span>
                <span>{{ getDurationDisplay(schedule) }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Actions -->
        <div class="card-actions" v-if="isFirstSlot(schedule)">
          <button @click.stop="$emit('delete', schedule)" 
                  class="delete-button">
            <span class="material-icons">close</span>
          </button>
        </div>

        <!-- Multiple Schedule Indicator -->
        <div v-if="schedules.length > 1 && index === 0" 
             class="multiple-indicator">
          {{ schedules.length }}
        </div>
      </div>
    </TransitionGroup>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { ScheduleDto } from '@/api/generated/data-contracts'

// Props and emits
const props = defineProps<{
  schedules: ScheduleDto[]
  currentTime: string
}>()

defineEmits<{
  (e: 'click', schedule: ScheduleDto): void
  (e: 'delete', schedule: ScheduleDto): void
}>()

// State
const expandedId = ref<number | null>(null)

// Methods
function toggleExpand(id: number) {
  expandedId.value = expandedId.value === id ? null : id
}

function getCardClasses(schedule: ScheduleDto) {
  const baseClass = 'bg-gradient-to-br'
  const colorClasses = [
    'from-blue-50 to-blue-100 border-blue-200',
    'from-emerald-50 to-emerald-100 border-emerald-200',
    'from-violet-50 to-violet-100 border-violet-200',
    'from-amber-50 to-amber-100 border-amber-200',
    'from-rose-50 to-rose-100 border-rose-200'
  ]
  return `${baseClass} ${colorClasses[schedule.studyGroupId % colorClasses.length]}`
}

function isFirstSlot(schedule: ScheduleDto): boolean {
  const scheduleStart = timeToMinutes(schedule.startTime?.toString() || '00:00')
  const currentSlot = timeToMinutes(props.currentTime)
  return scheduleStart === currentSlot
}

function timeToMinutes(time: string): number {
  const [hours, minutes] = time.split(':').map(Number)
  return hours * 60 + (minutes || 0)
}

function formatTime(time: string | undefined): string {
  return time?.substring(0, 5) || ''
}

function getDurationDisplay(schedule: ScheduleDto): string {
  const start = timeToMinutes(schedule.startTime?.toString() || '00:00')
  const end = timeToMinutes(schedule.endTime?.toString() || '00:00')
  return `${(end - start) / 60}h`
}
</script>

<style scoped>
.schedule-container {
  @apply h-full w-full overflow-hidden;
}

.schedule-card {
  @apply relative border rounded-lg p-2.5 cursor-pointer
         transition-all duration-200 shadow-sm hover:shadow-md;
  min-height: 3.5rem;
}

.card-content {
  @apply flex flex-col gap-2;
}

.primary-info {
  @apply flex justify-between items-start;
}

.group-info {
  @apply flex flex-col;
}

.group-name {
  @apply font-semibold text-sm truncate;
}

.subject-name {
  @apply text-xs text-gray-600 truncate;
}

.secondary-info {
  @apply flex items-center gap-2;
}

.room-badge {
  @apply px-2 py-0.5 text-xs font-medium rounded-full
         bg-white bg-opacity-50 shadow-sm;
}

.time-badge {
  @apply text-xs font-medium text-gray-600;
}

.expanded-content {
  @apply mt-2 pt-2 border-t border-gray-200;
}

.details-grid {
  @apply grid grid-cols-2 gap-2 text-xs;
}

.detail-item {
  @apply flex flex-col;
}

.detail-label {
  @apply text-gray-500 font-medium;
}

.card-actions {
  @apply absolute top-1 right-1 opacity-0 transition-opacity duration-200;
}

.delete-button {
  @apply p-1 rounded-full hover:bg-red-100 text-red-500;
}

.schedule-card:hover .card-actions {
  @apply opacity-100;
}

.multiple-indicator {
  @apply absolute -top-1 -right-1 w-5 h-5 rounded-full 
         bg-indigo-500 text-white text-xs font-medium
         flex items-center justify-center shadow-sm;
}

/* Animations */
.schedule-list-move,
.schedule-list-enter-active,
.schedule-list-leave-active {
  transition: all 0.3s ease;
}

.schedule-list-enter-from,
.schedule-list-leave-to {
  opacity: 0;
  transform: translateX(-30px);
}

.schedule-list-leave-active {
  position: absolute;
}

/* Responsive */
@media (max-width: 640px) {
  .schedule-card {
    @apply p-2;
  }
  
  .multiple-indicator {
    @apply w-4 h-4 text-[10px];
  }
}
</style>