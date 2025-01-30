<template>
  <div class="card-reader space-y-4 bg-white p-6 rounded-lg border">
    <!-- Card Input Field -->
    <div class="mb-4">
      <label class="block text-sm font-medium text-gray-700 mb-2">
        Card Reader Simulation
      </label>
      <input 
        type="text" 
        v-model="cardId"
        @keyup.enter="handleCardSwipe"
        ref="cardInput"
        class="w-full p-2 border rounded focus:ring-2 focus:ring-indigo-500"
        placeholder="Scan your card (Enter ID and press Enter)"
        :disabled="status === 'Processing'"
        autocomplete="off"
      />
    </div>

    <!-- Card Reader Visual Interface -->
    <div 
      class="text-center p-8 border-2 border-dashed rounded-lg transition-all duration-200 cursor-pointer"
      :class="{
        'border-indigo-500 bg-indigo-50': isInputFocused || status === 'Processing',
        'border-gray-300 hover:border-indigo-300': !isInputFocused && status === 'Ready',
        'border-green-500 bg-green-50': status === 'Success',
        'border-red-500 bg-red-50': status === 'Error'
      }"
      @click="focusInput"
    >
      <div class="space-y-4">
        <!-- Card Reader Icon -->
        <span class="material-icons text-6xl" :class="{
          'text-indigo-500 animate-pulse': status === 'Processing',
          'text-green-500': status === 'Success',
          'text-red-500': status === 'Error',
          'text-gray-400': status === 'Ready'
        }">
          {{ getStatusIcon }}
        </span>
        
        <!-- Status Message -->
        <div>
          <p class="text-lg font-medium" :class="{
            'text-indigo-600': status === 'Processing',
            'text-green-600': status === 'Success',
            'text-red-600': status === 'Error',
            'text-gray-600': status === 'Ready'
          }">
            {{ statusMessage }}
          </p>
          <p v-if="lastError" class="mt-2 text-sm text-red-600">{{ lastError }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useAttendanceStore } from '@/stores/attendance.store'

const props = defineProps<{
  deviceId: string
  courseSessionId: number
}>()

const attendanceStore = useAttendanceStore()
const cardId = ref('')
const status = ref('Ready')
const isInputFocused = ref(false)
const lastError = ref('')
const cardInput = ref<HTMLInputElement | null>(null)

const statusMessage = computed(() => {
  switch (status.value) {
    case 'Ready': return 'Ready to scan - Click here or tap card'
    case 'Processing': return 'Reading card...'
    case 'Success': return 'Attendance recorded successfully!'
    case 'Error': return 'Failed to record attendance - Try again'
    default: return 'Ready to scan'
  }
})

const getStatusIcon = computed(() => {
  switch (status.value) {
    case 'Processing': return 'sensors'
    case 'Success': return 'check_circle'
    case 'Error': return 'error'
    default: return 'contactless'
  }
})

async function handleCardSwipe() {
  if (!cardId.value || status.value === 'Processing') return
  
  status.value = 'Processing'
  lastError.value = ''
  
  try {
    await attendanceStore.recordCardAttendance({
      cardId: cardId.value,
      deviceId: props.deviceId,
      courseSessionId: props.courseSessionId
    })
    status.value = 'Success'
  } catch (error: any) {
    status.value = 'Error'
    lastError.value = error.response?.data?.message || 'Card not recognized'
  }
  
  cardId.value = ''
  setTimeout(() => {
    status.value = 'Ready'
    lastError.value = ''
    focusInput()
  }, 2000)
}

function focusInput() {
  if (cardInput.value) {
    cardInput.value.focus()
  }
}

// Maintain focus
function handleVisibilityChange() {
  if (!document.hidden) {
    focusInput()
  }
}

onMounted(() => {
  focusInput()
  document.addEventListener('visibilitychange', handleVisibilityChange)
  if (cardInput.value) {
    cardInput.value.addEventListener('focus', () => isInputFocused.value = true)
    cardInput.value.addEventListener('blur', () => isInputFocused.value = false)
  }
})

onUnmounted(() => {
  document.removeEventListener('visibilitychange', handleVisibilityChange)
})
</script>