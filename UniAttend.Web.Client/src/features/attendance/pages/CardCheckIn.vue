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
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useClassroomStore } from '@/stores/classroom.store'
import { useCourseSessionStore } from '@/stores/courseSession.store'
import CardReader from '../components/CardReader.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import type { TimeSpan } from '@/api/generated/data-contracts'

const route = useRoute()
const classroomStore = useClassroomStore()
const courseSessionStore = useCourseSessionStore()

const selectedDeviceId = ref('')
const { classroomsWithReaders } = storeToRefs(classroomStore)
const { currentCourseSession: courseSession } = storeToRefs(courseSessionStore)

function formatTime(time: TimeSpan | undefined): string {
  if (!time) return '--:--'
  return `${String(time.hours).padStart(2, '0')}:${String(time.minutes).padStart(2, '0')}`
}

onMounted(async () => {
  const sessionId = Number(route.params.id)
  if (sessionId) {
    await courseSessionStore.getCourseSessionById(sessionId)
  }
  await classroomStore.fetchClassrooms()
})
</script>