<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">My Profile</h1>
    </div>

    <div v-if="message" :class="[
      'p-4 rounded-md',
      message.type === 'success' ? 'bg-green-50 text-green-700' : 'bg-red-50 text-red-700'
    ]">
      {{ message.text }}
    </div>

    <div class="bg-white shadow rounded-lg p-6">
      <ProfileForm
        :user="mappedUser"
        @update-profile="handleUpdateProfile"
        @update-password="handleUpdatePassword"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { useUserStore } from '@/stores/user.store'
import { useAuthStore } from '@/stores/auth.store'
import type { UserProfileDto, UserDto, ChangePasswordCommand } from '@/api/generated/data-contracts'
import { StringRole } from '@/types/base.types'
import ProfileForm from '../components/ProfileForm.vue'

const userStore = useUserStore()
const authStore = useAuthStore()

const message = ref<{ text: string; type: 'success' | 'error' } | null>(null)

const currentUser = computed(() => authStore.user)

const mappedUser = computed<UserDto | null>(() => {
  if (!currentUser.value) return null;
  
  const roleMap: Record<StringRole, number> = {
    'admin': 1,
    'secretary': 2,
    'professor': 3,
    'student': 4
  };

  return {
    ...currentUser.value,
    role: roleMap[currentUser.value.role as StringRole] || 1
  } as UserDto;
})

async function handleUpdateProfile(profileData: UserProfileDto) {
  try {
    await userStore.updateProfile(profileData)
    message.value = { text: 'Profile updated successfully', type: 'success' }
  } catch (err) {
    console.error('Failed to update profile:', err)
    message.value = { text: 'Failed to update profile', type: 'error' }
  }
}

async function handleUpdatePassword(passwordData: ChangePasswordCommand) {
  try {
    await userStore.changePassword(passwordData)
    message.value = { text: 'Password changed successfully', type: 'success' }
  } catch (err) {
    console.error('Failed to change password:', err)
    message.value = { text: 'Failed to change password', type: 'error' }
  }
}

// Clear message after 5 seconds
watch(message, (newMessage) => {
  if (newMessage) {
    setTimeout(() => {
      message.value = null
    }, 5000)
  }
})
</script>