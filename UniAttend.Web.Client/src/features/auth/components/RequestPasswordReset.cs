<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <div>
      <label for="email" class="block text-sm font-medium text-gray-700">
        Email address
      </label>
      <div class="mt-1">
        <input
          id="email"
          v-model="email"
          type="email"
          required
          class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        />
      </div>
    </div>

    <!-- Success/Error messages -->
    <div v-if="message" :class="[
      'text-sm',
      success ? 'text-green-600' : 'text-red-600'
    ]">
      {{ message }}
    </div>

    <div>
      <button
        type="submit"
        :disabled="isLoading"
        class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
      >
        {{ isLoading ? 'Sending...' : 'Reset Password' }}
      </button>
    </div>

    <div class="text-center">
      <router-link
        to="/login"
        class="text-sm font-medium text-indigo-600 hover:text-indigo-500"
      >
        Back to login
      </router-link>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth.store'

const authStore = useAuthStore()
const email = ref('')
const isLoading = ref(false)
const message = ref('')
const success = ref(false)

async function handleSubmit() {
  isLoading.value = true
  message.value = ''
  success.value = false

  try {
    await authStore.requestPasswordReset(email.value)
    success.value = true
    message.value = 'Password reset instructions have been sent to your email'
    email.value = ''
  } catch (err: any) {
    message.value = err?.response?.data?.message || 'Failed to request password reset'
  } finally {
    isLoading.value = false
  }
}
</script>