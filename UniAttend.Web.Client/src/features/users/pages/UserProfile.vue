<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">My Profile</h1>
    </div>

    <div class="bg-white shadow rounded-lg p-6">
      <ProfileForm
        :user="currentUser"
        @submit="handleUpdateProfile"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useUserStore } from '@/stores/user.store'
import { useAuthStore } from '@/stores/auth.store'
import type { UpdateProfileRequest } from '@/types/user.types'
import ProfileForm from '../components/ProfileForm.vue'

const userStore = useUserStore()
const authStore = useAuthStore()

const currentUser = computed(() => authStore.user)

async function handleUpdateProfile(profileData: UpdateProfileRequest) {
  try {
    await userStore.updateProfile(profileData)
    // Show success message or handle success case
  } catch (err) {
    console.error('Failed to update profile:', err)
    // Show error message
  }
}
</script>