<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Schedule Management</h1>
      <Button @click="openCreateModal">Add Schedule</Button>
    </div>

    <!-- Filters -->
    <div class="flex gap-4 bg-white p-4 rounded-lg shadow">
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Group</label>
        <select
          v-model="selectedGroup"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All Groups</option>
          <option v-for="group in groups" :key="group.id" :value="group.id">
            {{ group.name }}
          </option>
        </select>
      </div>
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Classroom</label>
        <select
          v-model="selectedClassroom"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All Classrooms</option>
          <option v-for="classroom in classrooms" :key="classroom.id" :value="classroom.id">
            {{ classroom.name }}
          </option>
        </select>
      </div>
    </div>

    <!-- Schedule Grid -->
    <div class="bg-white shadow rounded-lg p-6">
      <div class="grid grid-cols-6 gap-4">
        <!-- Time Column -->
        <div class="col-span-1">
          <div class="h-12"></div>
          <div v-for="time in timeSlots" :key="time" class="h-20 flex items-center">
            {{ time }}
          </div>
        </div>

        <!-- Days Columns -->
        <div v-for="day in days" :key="day.value" class="col-span-1">
          <div class="h-12 flex items-center justify-center font-medium">
            {{ day.label }}
          </div>
          <div
            v-for="time in timeSlots"
            :key="`${day.value}-${time}`"
            class="h-20 border border-gray-200 p-2"
          >
            <ScheduleSlot
              :schedules="getSchedulesForSlot(day.value, time)"
              @click="slot => handleSlotClick(slot)"
            />
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
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useScheduleStore } from '@/stores/schedule.store'
import { useGroupStore } from '@/stores/studyGroup.store'
import { useClassroomStore } from '@/stores/classroom.store'
import type { Schedule } from '@/types/schedule.types'
import Button from '@/shared/components/ui/Button.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import ScheduleForm from '../components/ScheduleForm.vue'
import ScheduleSlot from '../components/ScheduleSlot.vue'

// Initialize stores
const scheduleStore = useScheduleStore()
const groupStore = useGroupStore()
const classroomStore = useClassroomStore()

// Get store refs
const { schedules } = storeToRefs(scheduleStore)
const { groups } = storeToRefs(groupStore)
const { classrooms } = storeToRefs(classroomStore)

// Component state
const showModal = ref(false)
const selectedSchedule = ref<Schedule | null>(null)
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
    filtered = filtered.filter(s => s.groupId === Number(selectedGroup.value))
  }
  
  if (selectedClassroom.value) {
    filtered = filtered.filter(s => s.classroomId === Number(selectedClassroom.value))
  }
  
  return filtered
})

// Methods
function getSchedulesForSlot(day: string, time: string) {
  return filteredSchedules.value.filter(schedule => 
    schedule.dayOfWeek.toString().toLowerCase() === day &&
    schedule.startTime === time
  )
}

function openCreateModal() {
  selectedSchedule.value = null
  showModal.value = true
}

function handleSlotClick(schedule: Schedule) {
  selectedSchedule.value = schedule
  showModal.value = true
}

async function handleSubmit(scheduleData: Partial<Schedule>) {
  try {
    if (selectedSchedule.value?.id) {
      await scheduleStore.updateSchedule(selectedSchedule.value.id, scheduleData)
    } else {
      await scheduleStore.createSchedule(scheduleData)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save schedule:', err)
  }
}

onMounted(async () => {
  await Promise.all([
    scheduleStore.fetchSchedules(),
    groupStore.fetchGroups(),
    classroomStore.fetchClassrooms()
  ])
})
</script>

<style scoped>
.schedule-grid {
  display: grid;
  grid-template-columns: 100px repeat(5, 1fr);
  gap: 1px;
  background-color: #e5e7eb;
}

.schedule-cell {
  background-color: white;
  padding: 0.5rem;
  min-height: 5rem;
}

.schedule-header {
  background-color: #f3f4f6;
  padding: 0.5rem;
  text-align: center;
  font-weight: 500;
}

.time-slot {
  background-color: #f3f4f6;
  padding: 0.5rem;
  text-align: right;
}
</style>