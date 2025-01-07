<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- Group Selection -->
    <div>
      <label for="groupId" class="block text-sm font-medium text-gray-700">Group</label>
      <select
        id="groupId"
        v-model="form.groupId"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option v-for="group in groups" :key="group.id" :value="group.id">
          {{ group.name }}
        </option>
      </select>
    </div>

    <!-- Classroom Selection -->
    <div>
      <label for="classroomId" class="block text-sm font-medium text-gray-700">Classroom</label>
      <select
        id="classroomId"
        v-model="form.classroomId"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option v-for="classroom in classrooms" :key="classroom.id" :value="classroom.id">
          {{ classroom.name }}
        </option>
      </select>
    </div>

    <!-- Day of Week -->
    <div>
      <label for="dayOfWeek" class="block text-sm font-medium text-gray-700">Day</label>
      <select
        id="dayOfWeek"
        v-model="form.dayOfWeek"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option :value="dayMapping.monday">Monday</option>
        <option :value="dayMapping.tuesday">Tuesday</option>
        <option :value="dayMapping.wednesday">Wednesday</option>
        <option :value="dayMapping.thursday">Thursday</option>
        <option :value="dayMapping.friday">Friday</option>
      </select>
    </div>

    <!-- Time Selection -->
    <div class="grid grid-cols-2 gap-4">
      <div>
        <label for="startTime" class="block text-sm font-medium text-gray-700">Start Time</label>
        <select
          id="startTime"
          v-model="form.startTime"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option v-for="time in timeSlots" :key="time" :value="time">{{ time }}</option>
        </select>
      </div>
      <div>
        <label for="duration" class="block text-sm font-medium text-gray-700">Duration (hours)</label>
        <input
          id="duration"
          v-model="form.duration"
          type="number"
          min="1"
          max="4"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
    </div>

    <div class="flex justify-end space-x-3">
      <Button type="button" variant="secondary" @click="$emit('cancel')">Cancel</Button>
      <Button type="submit" :loading="isLoading">Save</Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import Button from '@/shared/components/ui/Button.vue'
import type { Schedule } from '@/types/schedule.types'
import type { StudyGroup } from '@/types/group.types'
import type { Classroom } from '@/types/classroom.types'

interface Props {
  schedule?: Schedule | null
  groups: StudyGroup[]
  classrooms: Classroom[]
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: Partial<Schedule>): void
  (e: 'cancel'): void
}>()

const isLoading = ref(false)
const timeSlots = [
  '08:00', '09:00', '10:00', '11:00', '12:00',
  '13:00', '14:00', '15:00', '16:00', '17:00'
]

// Map day strings to numbers
const dayMapping = {
  monday: 1,
  tuesday: 2,
  wednesday: 3,
  thursday: 4,
  friday: 5
} as const

const form = ref({
  groupId: 0,
  classroomId: 0,
  dayOfWeek: 1 as number,
  startTime: '08:00',
  duration: 1
})

const endTime = computed(() => {
  const [hours, minutes] = form.value.startTime.split(':').map(Number)
  const endHours = hours + form.value.duration
  return `${endHours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`
})

watch(() => props.schedule, (newSchedule) => {
  if (newSchedule) {
    const duration = calculateDuration(newSchedule.startTime, newSchedule.endTime)
    form.value = {
      groupId: newSchedule.groupId,
      classroomId: newSchedule.classroomId,
      dayOfWeek: newSchedule.dayOfWeek,
      startTime: newSchedule.startTime,
      duration
    }
  }
}, { immediate: true })

function calculateDuration(startTime: string, endTime: string): number {
  const [startHours] = startTime.split(':').map(Number)
  const [endHours] = endTime.split(':').map(Number)
  return endHours - startHours
}

async function handleSubmit() {
  try {
    isLoading.value = true
    emit('submit', {
      ...form.value,
      endTime: endTime.value
    })
  } finally {
    isLoading.value = false
  }
}
</script>