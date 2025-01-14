<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">My Profile</h1>
    </div>

    <div class="bg-white shadow rounded-lg p-6">
      <ProfileForm
        :user="mappedUser"
        @submit="handleUpdateProfile"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useUserStore } from '@/stores/user.store'
import { useAuthStore } from '@/stores/auth.store'
import type { UserProfileDto, UserDto } from '@/api/generated/data-contracts'
import { StringRole } from '@/types/base.types'
import ProfileForm from '../components/ProfileForm.vue'

const userStore = useUserStore()
const authStore = useAuthStore()

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
  } catch (err) {
    console.error('Failed to update profile:', err)
  }
}
</script>