<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <div class="grid grid-cols-2 gap-4">
      <!-- First Name -->
      <div>
        <label for="firstName" class="block text-sm font-medium text-gray-700">First Name</label>
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
        <label for="lastName" class="block text-sm font-medium text-gray-700">Last Name</label>
        <input
          id="lastName"
          v-model="form.lastName"
          type="text"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
    </div>

    <!-- Email -->
    <div>
      <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
      <input
        id="email"
        v-model="form.email"
        type="email"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <!-- Department -->
    <div>
      <label for="departmentId" class="block text-sm font-medium text-gray-700">Department</label>
      <select
        id="departmentId"
        v-model="form.departmentId"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option value="">Select Department</option>
        <option v-for="dept in departments" :key="dept.id" :value="dept.id">
          {{ dept.name }}
        </option>
      </select>
    </div>

    <!-- Active Status -->
    <div>
      <label class="flex items-center">
        <input
          type="checkbox"
          v-model="form.isActive"
          class="rounded border-gray-300 text-indigo-600 focus:ring-indigo-500"
        />
        <span class="ml-2 text-sm text-gray-600">Active</span>
      </label>
    </div>

    <!-- Error Message -->
    <div v-if="error" class="text-sm text-red-600">
      {{ error }}
    </div>

    <!-- Form Actions -->
    <div class="flex justify-end space-x-3">
      <Button type="button" variant="secondary" @click="$emit('cancel')">Cancel</Button>
      <Button type="submit" :loading="isLoading">Save</Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import Button from '@/shared/components/ui/Button.vue'
import type { Department } from '@/types/department.types'
import type { Professor } from '@/types/professor.types'

interface Props {
  professor?: Professor | null
  departments: Department[]
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: Partial<Professor>): void
  (e: 'cancel'): void
}>()

const isLoading = ref(false)
const error = ref('')

const form = ref({
  firstName: '',
  lastName: '',
  email: '',
  departmentId: 0,
  isActive: true,
})

// Watch for changes in professor prop to update form
watch(() => props.professor, (newProfessor) => {
  if (newProfessor) {
    form.value = {
      firstName: newProfessor.firstName,
      lastName: newProfessor.lastName,
      email: newProfessor.email,
      departmentId: newProfessor.departmentId,
      isActive: newProfessor.isActive
    }
  } else {
    // Reset form for new professor
    form.value = {
      firstName: '',
      lastName: '',
      email: '',
      departmentId: 0,
      isActive: true
    }
  }
}, { immediate: true })

async function handleSubmit() {
  try {
    error.value = ''
    isLoading.value = true

    // Validate email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    if (!emailRegex.test(form.value.email)) {
      error.value = 'Please enter a valid email address'
      return
    }

    // Validate department selection
    if (!form.value.departmentId) {
      error.value = 'Please select a department'
      return
    }

    emit('submit', {
      ...form.value,
      departmentId: Number(form.value.departmentId)
    })
  } catch (err) {
    console.error('Form submission error:', err)
    error.value = 'An error occurred while saving the professor'
  } finally {
    isLoading.value = false
  }
}
</script>