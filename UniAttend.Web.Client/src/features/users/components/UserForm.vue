<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- User Info Section -->
    <div class="grid grid-cols-2 gap-4">
      <!-- Username field (only for new users) -->
      <div v-if="!props.user">
        <label for="username" class="block text-sm font-medium text-gray-700">Username</label>
        <input id="username" v-model="form.username" type="text" required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
      </div>

      <div>
        <label for="firstName" class="block text-sm font-medium text-gray-700">First Name</label>
        <input id="firstName" v-model="form.firstName" type="text" required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
      </div>

      <div>
        <label for="lastName" class="block text-sm font-medium text-gray-700">Last Name</label>
        <input id="lastName" v-model="form.lastName" type="text" required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
      </div>
    </div>

    <div>
      <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
      <input id="email" v-model="form.email" type="email" required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
    </div>

    <!-- Role selection (only for new users) -->
    <div v-if="!props.user">
      <label for="role" class="block text-sm font-medium text-gray-700">Role</label>
      <select id="role" v-model="form.role" required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
        <option value="">Select Role</option>
        <option :value="2">Secretary</option>
        <option :value="3">Professor</option>
      </select>
    </div>

    <!-- Department selection (only for professors) -->
    <div v-if="!props.user && form.role === 3">
    <label for="departmentId" class="block text-sm font-medium text-gray-700">Department</label>
    <select id="departmentId" v-model="form.departmentId" required
      class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
      <option value="">Select Department</option>
      <option v-for="dept in props.departments" :key="dept.id" :value="dept.id">
        {{ dept.name }}
      </option>
    </select>
  </div>

    <!-- Active Status (for existing users only) -->
    <div v-if="props.user">
      <label class="flex items-center space-x-2">
        <input type="checkbox" v-model="form.isActive"
          class="rounded border-gray-300 text-indigo-600 shadow-sm focus:border-indigo-500 focus:ring-indigo-500">
        <span class="text-sm font-medium text-gray-700">Active</span>
      </label>
    </div>

    <div v-if="error" class="text-sm text-red-600">
      {{ error }}
    </div>

    <div class="flex justify-end space-x-3">
      <Button type="button" variant="secondary" @click="$emit('cancel')">Cancel</Button>
      <Button type="submit" :loading="isLoading">
        {{ props.user ? 'Update User' : 'Create User' }}
      </Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import Button from '@/shared/components/ui/Button.vue'
import type {
  UserDto,
  DepartmentDto,
  CreateUserCommand,
  UpdateUserCommand,
  UserRole // Add this import
} from '@/api/generated/data-contracts'

interface Props {
  user?: UserDto | null
  departments: DepartmentDto[]
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: CreateUserCommand | UpdateUserCommand): void
  (e: 'cancel'): void
}>()

const isLoading = ref(false)
const error = ref('')

interface FormData {
  username: string
  firstName: string
  lastName: string
  email: string
  role: UserRole | null
  departmentId: number | null
  isActive: boolean
}

const form = ref<FormData>({
  username: '',
  firstName: '',
  lastName: '',
  email: '',
  role: null,
  departmentId: null,
  isActive: true
})

watch(() => props.user, (newUser) => {
  if (newUser) {
    form.value = {
      username: newUser.username ?? '',
      firstName: newUser.firstName ?? '',
      lastName: newUser.lastName ?? '',
      email: newUser.email ?? '',
      role: newUser.role ?? null,
      departmentId: newUser.departmentId ?? null,
      isActive: newUser.isActive ?? true
    }
  } else {
    // Reset form for new user
    form.value = {
      username: '',
      firstName: '',
      lastName: '',
      email: '',
      role: null,
      departmentId: null,
      isActive: true
    }
  }
}, { immediate: true })

async function handleSubmit() {
  try {
    // Validate form based on role
    if (!form.value.role) {
      error.value = 'Please select a role'
      return
    }

    isLoading.value = true
    error.value = ''

    if (props.user) {
      // Update existing user
      emit('submit', {
        id: props.user.id,
        firstName: form.value.firstName,
        lastName: form.value.lastName,
        email: form.value.email,
        departmentId: form.value.role === 3 ? form.value.departmentId : undefined,
        isActive: form.value.isActive
      } as UpdateUserCommand)
    } else {
      // Create new user
      const createData: CreateUserCommand = {
        username: form.value.username,
        firstName: form.value.firstName,
        lastName: form.value.lastName,
        email: form.value.email,
        role: form.value.role,
        departmentId: form.value.role === 3 ? form.value.departmentId! : undefined
      }
      emit('submit', createData)
    }
  } catch (err) {
    console.error('Form submission error:', err)
    error.value = 'Failed to submit form'
  } finally {
    isLoading.value = false
  }
}
</script>