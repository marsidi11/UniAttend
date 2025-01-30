<template>
  <div class="space-y-6">
    <header class="flex justify-between items-center">
      <h1 class="text-2xl font-bold">Card Check-in</h1>
    </header>

    <!-- Session Info -->
    <div class="bg-white p-6 rounded-lg shadow space-y-4">
      <div v-if="courseSession" class="grid grid-cols-2 gap-4">
        <div>
          <p class="text-sm text-gray-500">Study Group</p>
          <p class="font-medium">{{ courseSession.studyGroupName }}</p>
        </div>
        <div>
          <p class="text-sm text-gray-500">Classroom</p>
          <p class="font-medium">{{ courseSession.classroomName }}</p>
        </div>
        <div>
          <p class="text-sm text-gray-500">Time</p>
          <p class="font-medium">
            {{ formatTime(courseSession.startTime) }} - {{ formatTime(courseSession.endTime) }}
          </p>
        </div>
        <div>
          <p class="text-sm text-gray-500">Status</p>
          <Badge :status="courseSession.status === 'Active' ? 'success' : 'info'">
            {{ courseSession.status }}
          </Badge>
        </div>
      </div>

      <!-- Reader Selection -->
      <div class="form-group">
        <label for="deviceId" class="block text-sm font-medium text-gray-700">Card Reader</label>
        <select id="deviceId" v-model="selectedDeviceId"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
          <option value="">Select Reader Device</option>
          <option v-for="classroom in classroomsWithReaders" :key="classroom.id" :value="classroom.readerDeviceId">
            {{ classroom.name }} ({{ classroom.readerDeviceId }})
          </option>
        </select>
      </div>

      <!-- Card Reader Component -->
      <CardReader v-if="selectedDeviceId && courseSession?.status === 'Active' && courseSession.id"
        :device-id="selectedDeviceId" :course-session-id="courseSession.id" />
    </div>

    <div v-if="error" class="bg-red-50 border border-red-400 text-red-700 px-4 py-3 rounded relative">
      {{ error }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useClassroomStore } from '@/stores/classroom.store'
import { useCourseSessionStore } from '@/stores/courseSession.store'
import CardReader from '../components/CardReader.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import type { TimeSpan } from '@/api/generated/data-contracts'

const route = useRoute()
const router = useRouter() 
const classroomStore = useClassroomStore()
const courseSessionStore = useCourseSessionStore()

const selectedDeviceId = ref('')
const { classroomsWithReaders } = storeToRefs(classroomStore)
const { currentCourseSession: courseSession } = storeToRefs(courseSessionStore)

// Error handling state
const error = ref<string | null>(null)

function formatTime(time: TimeSpan | undefined): string {
  if (!time) return '--:--'
  return `${String(time.hours).padStart(2, '0')}:${String(time.minutes).padStart(2, '0')}`
}

onMounted(async () => {
  try {
    // Get session ID from route params and validate
    const sessionId = route.params.id
    console.log('Raw session ID from route:', sessionId)
    
    if (!sessionId || Array.isArray(sessionId)) {
      throw new Error('Invalid session ID')
    }

    const parsedSessionId = parseInt(sessionId, 10)
    console.log('Parsed session ID:', parsedSessionId)

    if (isNaN(parsedSessionId)) {
      throw new Error('Invalid session ID format')
    }

    // Fetch course session
    await courseSessionStore.getCourseSessionById(parsedSessionId)
    console.log('Fetched course session:', courseSession.value)

    // Fetch classrooms
    await classroomStore.fetchClassrooms()
    console.log('Available classrooms:', classroomsWithReaders.value)

  } catch (err: any) {
    console.error('Error in CardCheckIn setup:', err)
    error.value = err.message
    router.push('/dashboard') // Redirect on error
  }
})
</script>