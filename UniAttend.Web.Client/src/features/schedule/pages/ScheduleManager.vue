<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex flex-col sm:flex-row justify-between items-center gap-4">
      <h1 class="text-2xl font-bold text-gray-900">Schedule Management</h1>
      <Button @click="openCreateModal">Add Schedule</Button>
    </div>

    <!-- Filters -->
    <div class="flex flex-col sm:flex-row gap-4 bg-white p-4 rounded-lg shadow">
      <div class="w-full sm:w-64">
        <label class="block text-sm font-medium text-gray-700">Group</label>
        <select v-model="selectedGroup"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
          <option value="">All Groups</option>
          <option v-for="group in groups" :key="group.id" :value="group.id">
            {{ group.name }}
          </option>
        </select>
      </div>
      <div class="w-full sm:w-64">
        <label class="block text-sm font-medium text-gray-700">Classroom</label>
        <select v-model="selectedClassroom"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
          <option value="">All Classrooms</option>
          <option v-for="classroom in classrooms" :key="classroom.id" :value="classroom.id">
            {{ classroom.name }}
          </option>
        </select>
      </div>
    </div>

    <!-- Schedule Grid -->
    <div class="bg-white shadow rounded-lg p-4 sm:p-6 overflow-x-auto">
      <div v-if="!selectedGroup && !selectedClassroom" 
           class="text-center py-12 text-gray-500">
        Please select a group or classroom to view schedules
      </div>
      <div v-else-if="isLoading" 
           class="text-center py-12">
        <Spinner />
      </div>
      
      <!-- Schedule Grid Container -->
      <div class="min-w-[768px]"> <!-- Minimum width to prevent squishing -->
        <div class="grid grid-cols-6 gap-2 sm:gap-4">
          <!-- Time Column -->
          <div class="col-span-1 relative">
            <div class="h-14 sm:h-16"></div> <!-- Taller header space -->
            <div v-for="time in timeSlots" 
                 :key="time" 
                 class="h-24 sm:h-28 flex items-center justify-end pr-4 text-sm font-medium text-gray-500">
              <div class="bg-gray-50 px-2 py-1 rounded">
                {{ time }}
              </div>
            </div>
          </div>

          <!-- Days Columns -->
          <div v-for="day in days" 
               :key="day.value" 
               class="col-span-1">
            <div class="h-14 sm:h-16 flex items-center justify-center font-medium bg-indigo-50 rounded-t text-indigo-700">
              <span class="hidden sm:inline">{{ day.label }}</span>
              <span class="sm:hidden">{{ day.label.substr(0, 3) }}</span>
            </div>
            <div v-for="time in timeSlots" 
                 :key="`${day.value}-${time}`" 
                 class="h-24 sm:h-28 border border-gray-100 hover:bg-gray-50 transition-colors duration-200 p-1 sm:p-2 rounded">
              <ScheduleSlot 
                :schedules="getSchedulesForSlot(day.value, time)" 
                @click="slot => handleSlotClick(slot)" 
                @delete="handleDelete"
              />
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Create/Edit Schedule Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <ScheduleForm 
        v-if="showModal" 
        :schedule="selectedSchedule" 
        :groups="groups" 
        :classrooms="classrooms"
        @submit="handleSubmit" 
        @cancel="showModal = false" 
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { useScheduleStore } from '@/stores/schedule.store'
import { useGroupStore } from '@/stores/studyGroup.store'
import { useClassroomStore } from '@/stores/classroom.store'
import type {
  ScheduleDto,
  CreateScheduleCommand,
  UpdateScheduleCommand
} from '@/api/generated/data-contracts'
import Button from '@/shared/components/ui/Button.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import ScheduleForm from '../components/ScheduleForm.vue'
import ScheduleSlot from '../components/ScheduleSlot.vue'

// Initialize stores
const scheduleStore = useScheduleStore()
const groupStore = useGroupStore()
const classroomStore = useClassroomStore()

// Get store refs
const { schedules, isLoading } = storeToRefs(scheduleStore)
const { groups } = storeToRefs(groupStore)
const { classrooms } = storeToRefs(classroomStore)

// Component state
const showModal = ref(false)
const selectedSchedule = ref<ScheduleDto | null>(null)
const selectedGroup = ref('')
const selectedClassroom = ref('')

// Constants
const days = [
  { value: 'monday', label: 'Monday' },
  { value: 'tuesday', label: 'Tuesday' },
  { value: 'wednesday', label: 'Wednesday' },
  { value: 'thursday', label: 'Thursday' },
  { value: 'friday', label: 'Friday' },
] as const

const timeSlots = [
  '08:00', '09:00', '10:00', '11:00', '12:00',
  '13:00', '14:00', '15:00', '16:00', '17:00'
]

// Computed properties
const modalTitle = computed(() =>
  selectedSchedule.value ? 'Edit Schedule' : 'Add Schedule'
)

const filteredSchedules = computed(() => {
  let filtered = [...schedules.value]

  if (selectedGroup.value) {
    filtered = filtered.filter(s => s.studyGroupId === Number(selectedGroup.value))
  }

  if (selectedClassroom.value) {
    filtered = filtered.filter(s => s.classroomId === Number(selectedClassroom.value))
  }

  return filtered
})

// Methods
function getSchedulesForSlot(day: string, time: string) {
  return filteredSchedules.value.filter(schedule => {
    return schedule.dayOfWeek === getDayNumber(day) &&
      schedule.startTime === `${time}:00`
  })
}

// Add helper function to convert day string to number
function getDayNumber(day: string): number {
  const dayMap: Record<string, number> = {
    'monday': 1,
    'tuesday': 2,
    'wednesday': 3,
    'thursday': 4,
    'friday': 5
  }
  return dayMap[day]
}

function openCreateModal() {
  selectedSchedule.value = null
  showModal.value = true
}

function handleSlotClick(schedule: ScheduleDto) {
  selectedSchedule.value = schedule
  showModal.value = true
}

async function handleDelete(schedule: ScheduleDto) {
  if (!schedule.id || !schedule.studyGroupId) {
    console.error('Schedule has no ID or study group ID')
    return
  }

  if (!confirm('Are you sure you want to delete this schedule?')) return
  
  try {
    await scheduleStore.deleteSchedule(
      schedule.id, 
      schedule.studyGroupId 
    )
  } catch (err) {
    console.error('Failed to delete schedule:', err)
  }
}

async function handleSubmit(scheduleData: CreateScheduleCommand | UpdateScheduleCommand) {
  try {
    if (selectedSchedule.value?.id) {
      await scheduleStore.updateSchedule(
        selectedSchedule.value.id,
        scheduleData as UpdateScheduleCommand
      )
    } else {
      await scheduleStore.createSchedule(scheduleData as CreateScheduleCommand)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save schedule:', err)
  }
}

// Watch for filter changes
watch(selectedGroup, async (newValue) => {
  try {
    if (newValue) {
      // Group selected
      await scheduleStore.fetchSchedules(Number(newValue))
    } else if (selectedClassroom.value) {
      // Group deselected but classroom is selected
      await scheduleStore.fetchSchedules(undefined, Number(selectedClassroom.value))
    } else {
      // Both filters cleared
      schedules.value = []
    }
  } catch (err) {
    console.error('Failed to fetch group schedules:', err)
  }
})

watch(selectedClassroom, async (newValue) => {
  try {
    if (newValue) {
      // Classroom selected
      await scheduleStore.fetchSchedules(undefined, Number(newValue))
    } else if (selectedGroup.value) {
      // Classroom deselected but group is selected
      await scheduleStore.fetchSchedules(Number(selectedGroup.value))
    } else {
      // Both filters cleared
      schedules.value = []
    }
  } catch (err) {
    console.error('Failed to fetch classroom schedules:', err)
  }
})

// Modify onMounted to only fetch groups and classrooms
onMounted(async () => {
  await Promise.all([
    groupStore.fetchGroups(),
    classroomStore.fetchClassrooms()
  ])
})
</script>

<style scoped>
.schedule-grid {
  scrollbar-width: thin;
  scrollbar-color: #4f46e5 #f3f4f6;
}

.schedule-grid::-webkit-scrollbar {
  height: 6px;
}

.schedule-grid::-webkit-scrollbar-track {
  background: #f3f4f6;
  border-radius: 3px;
}

.schedule-grid::-webkit-scrollbar-thumb {
  background-color: #4f46e5;
  border-radius: 3px;
}

@media (max-width: 640px) {
  .schedule-grid {
    margin: 0 -1rem;
  }
}
</style>