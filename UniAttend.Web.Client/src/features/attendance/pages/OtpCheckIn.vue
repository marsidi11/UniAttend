<template>
  <div class="min-h-screen bg-gray-100 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md mx-auto">
      <div class="text-center mb-8">
        <h2 class="text-3xl font-extrabold text-gray-900">
          Check-in with OTP
        </h2>
        <p class="mt-2 text-sm text-gray-600">
          Enter the OTP code provided by your professor
        </p>
      </div>

      <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
        <!-- Error Message -->
        <div v-if="error" class="mb-4 p-4 bg-red-50 text-red-700 rounded-md">
          {{ error }}
        </div>

        <!-- Success Message -->
        <div 
          v-if="isSuccess" 
          class="mb-4 p-4 bg-green-50 text-green-700 rounded-md flex items-center"
        >
          <span class="material-icons mr-2">check_circle</span>
          Check-in successful!
        </div>

        <form @submit.prevent="handleSubmit" class="space-y-6">
          <!-- Class ID -->
          <div>
            <label for="courseSessionId" class="block text-sm font-medium text-gray-700">
              Class ID
            </label>
            <input
              id="courseSessionId"
              v-model="form.courseSessionId"
              type="number"
              required
              class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
              :disabled="isLoading"
            />
          </div>

          <!-- OTP Code -->
          <div>
            <label for="otpCode" class="block text-sm font-medium text-gray-700">
              OTP Code
            </label>
            <input
              id="otpCode"
              v-model="form.otpCode"
              type="text"
              required
              maxlength="6"
              pattern="\d{6}"
              class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm tracking-widest text-center text-2xl"
              :disabled="isLoading"
              @input="handleOtpInput"
            />
            <p class="mt-1 text-sm text-gray-500">
              Enter the 6-digit code
            </p>
          </div>

          <div>
            <Button
              type="submit"
              :loading="isLoading"
              :disabled="!isFormValid"
              class="w-full"
            >
              Check In
            </Button>
          </div>
        </form>

        <div class="mt-6">
          <Button
            variant="secondary"
            class="w-full"
            @click="$router.push('/dashboard')"
          >
            Back to Dashboard
          </Button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAttendanceStore } from '@/stores/attendance.store'
import Button from '@/shared/components/ui/Button.vue'
import { VerificationType } from '@/api/generated/data-contracts'

const router = useRouter()
const attendanceStore = useAttendanceStore()

const isLoading = ref(false)
const error = ref('')
const isSuccess = ref(false)

const form = ref({
  courseSessionId: '',
  otpCode: ''
})

const isFormValid = computed(() => {
  return form.value.courseSessionId && 
         form.value.otpCode && 
         form.value.otpCode.length === 6
})

function handleOtpInput(event: Event) {
  const input = event.target as HTMLInputElement
  input.value = input.value.replace(/\D/g, '')
  form.value.otpCode = input.value
}

async function handleSubmit() {
  if (!isFormValid.value) return

  isLoading.value = true
  error.value = ''
  isSuccess.value = false

  try {
    await attendanceStore.recordOtpAttendance({
      courseSessionId: Number(form.value.courseSessionId),
      otpCode: form.value.otpCode,
      verificationType: VerificationType.Value1, // TOTP
      studentId: 0 // Will be set by backend from token
    })
    
    isSuccess.value = true
    
    setTimeout(() => {
      router.push('/dashboard/attendance')
    }, 2000)
  } catch (err: any) {
    error.value = err?.response?.data?.message || 'Failed to check in. Please try again.'
  } finally {
    isLoading.value = false
  }
}
</script>

<style scoped>
/* Optional: Style the OTP input to look more like a code input */
input[type="text"] {
  letter-spacing: 0.5em;
  font-family: monospace;
}
</style>