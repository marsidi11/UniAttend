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

    <ProfileForm :user="mappedUser" :totp-setup="totpSetup" @update-profile="handleUpdateProfile"
      @update-password="handleUpdatePassword" @setup-totp="handleSetupTotp" @confirm-totp="handleConfirmTotp" />
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watch, onMounted } from 'vue'
import { useUserStore } from '@/stores/user.store'
import type { 
  UserProfileDto, 
  UserDto, 
  ChangePasswordCommand,
  TotpSetupDto 
} from '@/api/generated/data-contracts'
import ProfileForm from '../components/ProfileForm.vue'

const userStore = useUserStore()

const isLoading = ref(true)
const profileData = ref<UserProfileDto | null>(null)

const message = ref<{ text: string; type: 'success' | 'error' } | null>(null)

const totpSetup = ref<TotpSetupDto | null>(null)

const mappedUser = computed<UserDto | null>(() => {
  if (!profileData.value) return null;
  
  return {
    id: profileData.value.id,
    username: profileData.value.username,
    email: profileData.value.email,
    firstName: profileData.value.firstName,
    lastName: profileData.value.lastName,
    role: profileData.value.role,
    isTwoFactorEnabled: profileData.value.isTwoFactorEnabled,
    isTwoFactorVerified: profileData.value.isTwoFactorVerified
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
    const setup = await userStore.setupTotp()
    totpSetup.value = setup
  } catch (err) {
    console.error('Failed to setup TOTP:', err)
    message.value = { text: 'Failed to setup two-factor authentication', type: 'error' }
  }
}

async function handleConfirmTotp(code: string) {
  try {
    await userStore.verifyTotp(code)
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

onMounted(async () => {
  try {
    profileData.value = await userStore.fetchProfile()
  } catch (err) {
    console.error('Failed to fetch profile:', err)
    message.value = { text: 'Failed to load profile data', type: 'error' }
  } finally {
    isLoading.value = false
  }
})
</script>