<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <div class="grid grid-cols-2 gap-4">
      <!-- First Name -->
      <div>
        <label for="firstName" class="block text-sm font-medium text-gray-700">
          First Name
        </label>
        <input
          id="firstName"
          v-model="form.firstName"
          type="text"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>

      <!-- Last Name -->
      <div>
        <label for="lastName" class="block text-sm font-medium text-gray-700">
          Last Name
        </label>
        <input
          id="lastName"
          v-model="form.lastName"
          type="text"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
    </div>

    <!-- Username -->
    <div>
      <label for="username" class="block text-sm font-medium text-gray-700">
        Username
      </label>
      <input
        id="username"
        v-model="form.username"
        type="text"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <!-- Email -->
    <div>
      <label for="email" class="block text-sm font-medium text-gray-700">
        Email
      </label>
      <input
        id="email"
        v-model="form.email"
        type="email"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <!-- Password -->
    <div>
      <label for="password" class="block text-sm font-medium text-gray-700">
        Password
      </label>
      <input
        id="password"
        v-model="form.password"
        type="password"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <!-- Role -->
    <div>
      <label for="role" class="block text-sm font-medium text-gray-700">
        Role
      </label>
      <select
        id="role"
        v-model="form.role"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option value="student">Student</option>
        <option value="professor">Professor</option>
      </select>
    </div>

    <!-- Student-specific fields -->
    <template v-if="form.role === 'student'">
      <div>
        <label for="studentId" class="block text-sm font-medium text-gray-700">
          Student ID
        </label>
        <input
          id="studentId"
          v-model="form.studentId"
          type="text"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>

      <div>
        <label for="departmentId" class="block text-sm font-medium text-gray-700">
          Department
        </label>
        <select
          id="departmentId"
          v-model="form.departmentId"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option v-for="dept in departments" :key="dept.id" :value="dept.id">
            {{ dept.name }}
          </option>
        </select>
      </div>
    </template>

    <!-- Error message -->
    <div v-if="error" class="text-red-600 text-sm">
      {{ error }}
    </div>

    <div>
      <button
        type="submit"
        :disabled="isLoading"
        class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
      >
        {{ isLoading ? 'Creating account...' : 'Create account' }}
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { storeToRefs } from 'pinia' 
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import type { RegisterRequest } from '@/types/auth.types'
import { useDepartmentStore } from '@/stores/department.store'

const router = useRouter()
const authStore = useAuthStore()
const departmentStore = useDepartmentStore()
const { departments } = storeToRefs(departmentStore) 
const isLoading = ref(false)
const error = ref('')

const form = ref<RegisterRequest>({
  username: '',
  password: '',
  email: '',
  firstName: '',
  lastName: '',
  role: 'student',
  studentId: '',
  departmentId: undefined,
  cardId: undefined
})

onMounted(async () => {
  try {
    await departmentStore.fetchDepartments()
  } catch (err) {
    console.error('Failed to load departments:', err)
  }
})

async function handleSubmit() {
  error.value = ''
  isLoading.value = true
  
  try {
    await authStore.register(form.value)
    router.push('/dashboard')
  } catch (err: any) {
    error.value = err?.response?.data?.message || 'Registration failed'
  } finally {
    isLoading.value = false
  }
}
</script>