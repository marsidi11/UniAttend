<template>
  <div class="space-y-4">
    <!-- Test input - visible for development -->
    <div class="mb-4 p-4 bg-gray-50 rounded-lg">
      <input 
        type="text" 
        v-model="cardId"
        @keyup.enter="handleCardSwipe"
        ref="cardInput"
        class="w-full p-2 border rounded"
        placeholder="Enter card ID and press Enter"
        :disabled="status === 'Processing'"
        autocomplete="off"
      />
    </div>

    <!-- Card Reader Visual Interface -->
    <div 
      class="text-center p-8 border-2 border-dashed rounded-lg transition-colors cursor-pointer"
      :class="{
        'border-indigo-500 bg-indigo-50': isInputFocused,
        'border-gray-300 hover:border-indigo-300': !isInputFocused,
        'border-red-500 bg-red-50': status === 'Error'
      }"
      @click="focusInput"
    >
      <div class="space-y-4">
        <span class="material-icons text-6xl" :class="{
          'text-indigo-500': isInputFocused,
          'text-gray-400': !isInputFocused && status !== 'Error',
          'text-red-500': status === 'Error'
        }">
          {{ status === 'Processing' ? 'sensors' : 'contactless' }}
        </span>
        
        <div>
          <p class="text-lg font-medium" :class="{
            'text-indigo-600': isInputFocused,
            'text-gray-600': !isInputFocused && status !== 'Error',
            'text-red-600': status === 'Error'
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
    case 'Ready': return 'Click here or tap card to scan'
    case 'Processing': return 'Processing...'
    case 'Success': return 'Card accepted!'
    case 'Error': return 'Scan failed - Try again'
    default: return 'Ready to scan'
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
  } catch (error) {
    status.value = 'Error'
    lastError.value = 'Card not recognized'
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