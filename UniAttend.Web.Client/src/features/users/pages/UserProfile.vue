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
        :totp-setup="totpSetup"
        @update-profile="handleUpdateProfile"
        @update-password="handleUpdatePassword"
        @setup-totp="handleSetupTotp"
        @confirm-totp="handleConfirmTotp"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { useUserStore } from '@/stores/user.store'
import { useAuthStore } from '@/stores/auth.store'
import { useAttendanceStore } from '@/stores/attendance.store'
import type { 
  UserProfileDto, 
  UserDto, 
  ChangePasswordCommand,
  TotpSetupDto 
} from '@/api/generated/data-contracts'
import { StringRole } from '@/types/base.types'
import ProfileForm from '../components/ProfileForm.vue'

const userStore = useUserStore()
const authStore = useAuthStore()
const attendanceStore = useAttendanceStore()

const message = ref<{ text: string; type: 'success' | 'error' } | null>(null)

const currentUser = computed(() => authStore.user)

const totpSetup = ref<TotpSetupDto | null>(null)

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

async function handleSetupTotp() {
  try {
    const setup = await attendanceStore.setupTotp()
    totpSetup.value = setup
  } catch (err) {
    console.error('Failed to setup TOTP:', err)
    message.value = { text: 'Failed to setup two-factor authentication', type: 'error' }
  }
}

async function handleConfirmTotp(code: string) {
  try {
    await attendanceStore.verifyTotp(code)
    totpSetup.value = null
    message.value = { text: 'Two-factor authentication enabled successfully', type: 'success' }
    await userStore.fetchProfile()
  } catch (err) {
    console.error('Failed to confirm TOTP setup:', err)
    throw err // Re-throw to handle in form component
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