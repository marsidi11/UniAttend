<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- User Info Section -->
    <div class="grid grid-cols-2 gap-4">
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

    <!-- Password Change Section -->
    <div class="pt-6 border-t">
      <h3 class="text-lg font-medium text-gray-900 mb-4">Change Password</h3>
      <div class="space-y-4">
        <div>
          <label for="currentPassword" class="block text-sm font-medium text-gray-700">Current Password</label>
          <input 
            id="currentPassword" 
            v-model="passwordForm.currentPassword" 
            type="password"
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" 
          />
        </div>
        <div>
          <label for="newPassword" class="block text-sm font-medium text-gray-700">New Password</label>
          <input 
            id="newPassword" 
            v-model="passwordForm.newPassword" 
            type="password"
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" 
          />
        </div>
        <div>
          <label for="confirmPassword" class="block text-sm font-medium text-gray-700">Confirm Password</label>
          <input 
            id="confirmPassword" 
            v-model="passwordForm.confirmPassword" 
            type="password"
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" 
          />
        </div>
      </div>
    </div>

    <div v-if="error" class="text-sm text-red-600">
      {{ error }}
    </div>

    <div class="flex justify-end space-x-3">
      <Button type="submit" :loading="isLoading">Save Changes</Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import Button from '@/shared/components/ui/Button.vue'
import type { 
  UserDto,
  UserProfileDto,
  UpdateProfileCommand,
  ChangePasswordCommand 
} from '@/api/generated/data-contracts'

interface Props {
  user: UserDto | UserProfileDto | null;
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: UpdateProfileCommand & Partial<ChangePasswordCommand>): void;
}>()

const isLoading = ref(false)
const error = ref('')

const form = ref<Omit<UpdateProfileCommand, 'userId'>>({
  firstName: '',
  lastName: '',
  email: ''
})

const passwordForm = ref({
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
})

watch(() => props.user, (newUser) => {
  if (newUser) {
    form.value = {
      firstName: newUser.firstName ?? '',
      lastName: newUser.lastName ?? '',
      email: newUser.email ?? ''
    }
  }
}, { immediate: true })

async function handleSubmit() {
  try {
    isLoading.value = true
    error.value = ''

    // Validate password change if attempted
    if (passwordForm.value.newPassword || passwordForm.value.currentPassword || passwordForm.value.confirmPassword) {
      if (!passwordForm.value.currentPassword) {
        error.value = 'Current password is required to change password'
        return
      }
      if (passwordForm.value.newPassword !== passwordForm.value.confirmPassword) {
        error.value = 'New passwords do not match'
        return
      }
    }

    emit('submit', {
      ...form.value,
      ...(passwordForm.value.newPassword ? {
        currentPassword: passwordForm.value.currentPassword,
        newPassword: passwordForm.value.newPassword
      } : {})
    })
  } finally {
    isLoading.value = false
  }
}
</script>