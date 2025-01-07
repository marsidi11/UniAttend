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
  
      <!-- Role -->
      <div>
        <label for="role" class="block text-sm font-medium text-gray-700">Role</label>
        <select
          id="role"
          v-model="form.role"
          required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="admin">Admin</option>
          <option value="secretary">Secretary</option>
          <option value="professor">Professor</option>
          <option value="student">Student</option>
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
  
      <div class="flex justify-end space-x-3">
        <Button type="button" variant="secondary" @click="$emit('cancel')">Cancel</Button>
        <Button type="submit" :loading="isLoading">Save</Button>
      </div>
    </form>
  </template>
  
  <script setup lang="ts">
  import { ref, watch } from 'vue'
  import Button from '@/shared/components/ui/Button.vue'
  import type { User } from '@/types/user.types'
  import type { Role } from '@/types/base.types'
  
  interface Props {
    user?: User | null
  }
  
  interface UserFormData {
    firstName: string
    lastName: string
    email: string
    role: Role
    isActive: boolean
  }
  
  const props = defineProps<Props>()
  const emit = defineEmits<{
    (e: 'submit', data: Partial<UserFormData>): void
    (e: 'cancel'): void
  }>()
  
  const isLoading = ref(false)
  const form = ref<UserFormData>({
    firstName: '',
    lastName: '',
    email: '',
    role: 'Student',
    isActive: true
  })
  
  watch(() => props.user, (newUser) => {
    if (newUser) {
      form.value = {
        firstName: newUser.firstName,
        lastName: newUser.lastName,
        email: newUser.email,
        role: newUser.role as Role,
        isActive: newUser.isActive
      }
    }
  }, { immediate: true })
  
  async function handleSubmit() {
    try {
      isLoading.value = true
      emit('submit', form.value)
    } finally {
      isLoading.value = false
    }
  }
  </script>