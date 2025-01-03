<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- Username field -->
    <div>
      <label for="username" class="block text-sm font-medium text-gray-700">
        Username
      </label>
      <div class="mt-1">
        <input
          id="username"
          v-model="form.username"
          type="text"
          required
          class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        />
      </div>
    </div>

    <!-- Password field -->
    <div>
      <label for="password" class="block text-sm font-medium text-gray-700">
        Password
      </label>
      <div class="mt-1">
        <input
          id="password"
          v-model="form.password"
          type="password"
          required
          class="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        />
      </div>
    </div>

    <!-- Error message -->
    <div v-if="error" class="text-red-600 text-sm">
      {{ error }}
    </div>

    <div class="flex items-center justify-between">
      <div class="text-sm">
        <router-link
          to="/forgot-password"
          class="font-medium text-indigo-600 hover:text-indigo-500"
        >
          Forgot your password?
        </router-link>
      </div>
    </div>

    <div>
      <button
        type="submit"
        :disabled="isLoading"
        class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
      >
        {{ isLoading ? 'Signing in...' : 'Sign in' }}
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import type { LoginCredentials } from '@/types/auth.types'

const router = useRouter()
const authStore = useAuthStore()
const isLoading = ref(false)
const error = ref('')

const form = ref<LoginCredentials>({
  username: '',
  password: ''
})

async function handleSubmit() {
  error.value = ''
  isLoading.value = true
  
  try {
    await authStore.login(form.value)
    router.push('/dashboard')
  } catch (err: any) {
    error.value = err?.response?.data?.message || 'Failed to sign in'
  } finally {
    isLoading.value = false
  }
}
</script>