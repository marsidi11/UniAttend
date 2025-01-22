<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex flex-col sm:flex-row justify-between items-center gap-4">
      <h1 class="text-2xl font-bold text-gray-900">Schedule Management</h1>
      <Button @click="openCreateModal">Add Schedule</Button>
    </div>

    <!-- Enhanced Filters -->
    <div class="flex flex-col sm:flex-row gap-4 bg-white p-4 rounded-lg shadow">
      <!-- Study Group Filter -->
      <div class="w-full sm:w-1/4">
        <label class="block text-sm font-medium text-gray-700">Study Group</label>
        <select v-model="selectedGroup"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
          <option value="">All Groups</option>
          <option v-for="group in groups" :key="group.id" :value="group.id">
            {{ group.name }}
          </option>
        </select>
      </div>

      <!-- Classroom Filter -->
      <div class="w-full sm:w-1/4">
        <label class="block text-sm font-medium text-gray-700">Classroom</label>
        <select v-model="selectedClassroom"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
          <option value="">All Classrooms</option>
          <option v-for="classroom in classrooms" :key="classroom.id" :value="classroom.id">
            {{ classroom.name }}
          </option>
        </select>
      </div>

      <!-- Professor Filter -->
      <div class="w-full sm:w-1/4">
        <label class="block text-sm font-medium text-gray-700">Professor</label>
        <select v-model="selectedProfessor"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
          <option value="">All Professors</option>
          <option v-for="professor in professors" :key="professor.id" :value="professor.id">
            {{ professor.fullName }}
          </option>
        </select>
      </div>
    </div>

    <!-- Schedule Grid -->
    <div class="bg-white shadow rounded-lg p-4 sm:p-6 overflow-x-auto">
      <div v-if="isLoading" class="text-center py-12">
        <Spinner />
      </div>

      <!-- Schedule Grid Container -->
      <div v-else class="min-w-[768px]">
        <div class="grid grid-cols-6 gap-2 sm:gap-4">
          <!-- Time Column -->
          <div class="col-span-1 relative">
            <div class="h-14 sm:h-16"></div> <!-- Taller header space -->
            <div v-for="time in timeSlots" :key="time"
              class="h-24 sm:h-28 flex items-center justify-end pr-4 text-sm font-medium text-gray-500">
              <div class="bg-gray-50 px-2 py-1 rounded">
                {{ time }}
              </div>
            </div>
          </div>

          <!-- Days Columns -->
          <div v-for="day in days" :key="day.value" class="col-span-1">
            <div
              class="h-14 sm:h-16 flex items-center justify-center font-medium bg-indigo-50 rounded-t text-indigo-700">
              <span class="hidden sm:inline">{{ day.label }}</span>
              <span class="sm:hidden">{{ day.label.substr(0, 3) }}</span>
            </div>
            <div v-for="time in timeSlots" :key="`${day.value}-${time}`"
              class="h-24 sm:h-28 border border-gray-100 hover:bg-gray-50 transition-colors duration-200 p-1 sm:p-2 rounded">
              <ScheduleSlot :schedules="getSchedulesForSlot(day.value, time)" :current-time="`${time}:00`"
                @click="slot => handleSlotClick(slot)" @delete="handleDelete" />
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Create/Edit Schedule Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <ScheduleForm v-if="showModal" :schedule="selectedSchedule" :groups="groups" :classrooms="classrooms"
        @submit="handleSubmit" @cancel="showModal = false" />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { useScheduleStore } from '@/stores/schedule.store'
import { useGroupStore } from '@/stores/studyGroup.store'
import { useClassroomStore } from '@/stores/classroom.store'
import { useProfessorStore } from '@/stores/professor.store'
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
const professorStore = useProfessorStore()

// Get store refs
const { schedules, isLoading } = storeToRefs(scheduleStore)
const { groups } = storeToRefs(groupStore)
const { classrooms } = storeToRefs(classroomStore)
const { professors } = storeToRefs(professorStore)

// Component state
const showModal = ref(false)
const selectedSchedule = ref<ScheduleDto | null>(null)
const selectedGroup = ref('')
const selectedClassroom = ref('')
const selectedProfessor = ref('')

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

  if (selectedProfessor.value) {
    // Filter by professor using the professor name from group info
    filtered = filtered.filter(s => {
      const professorName = s.professorName?.toLowerCase() || ''
      const selectedProf = professors.value.find(p => p.id === Number(selectedProfessor.value))
      return professorName.includes(selectedProf?.fullName?.toLowerCase() || '')
    })
  }

  return filtered
})

// Methods
function getSchedulesForSlot(day: string, time: string) {
  const schedules = filteredSchedules.value.filter(schedule => {
    if (schedule.dayOfWeek !== getDayNumber(day)) return false
    const slotMinutes = timeToMinutes(time)
    const startMinutes = timeToMinutes(schedule.startTime?.toString() || '00:00')
    const endMinutes = timeToMinutes(schedule.endTime?.toString() || '00:00')
    return slotMinutes >= startMinutes && slotMinutes < endMinutes
  })

  // Sort schedules by type for consistent display order
  return schedules.sort((a, b) => {
    // Prioritize display order: groups -> classrooms -> professors
    const getTypeOrder = (s: ScheduleDto) => {
      if (s.studyGroupId) return 1
      if (s.classroomId) return 2
      return 3
    }
    return getTypeOrder(a) - getTypeOrder(b)
  })
}

// Helper function to convert time string to minutes
function timeToMinutes(time: string): number {
  const [hours, minutes] = time.split(':').map(Number);
  return (hours * 60) + (minutes || 0);
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
watch([selectedGroup, selectedClassroom, selectedProfessor], async ([group, classroom, professor]) => {
  try {
    if (professor) {
      // Use the updated fetchSchedules method that supports professor filtering
      await scheduleStore.fetchSchedules(undefined, undefined, Number(professor))
    } else if (classroom) {
      await scheduleStore.fetchSchedules(undefined, Number(classroom))
    } else if (group) {
      await scheduleStore.fetchSchedules(Number(group))
    } else {
      await scheduleStore.fetchAllSchedules()
    }
  } catch (err) {
    console.error('Failed to fetch schedules:', err)
  }
})

// Modify onMounted to only fetch groups and classrooms
onMounted(async () => {
  try {
    await Promise.all([
      scheduleStore.fetchAllSchedules(),
      groupStore.fetchGroups(),
      classroomStore.fetchClassrooms(),
      professorStore.fetchProfessors()
    ])
  } catch (err) {
    console.error('Failed to fetch initial data:', err)
  }
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