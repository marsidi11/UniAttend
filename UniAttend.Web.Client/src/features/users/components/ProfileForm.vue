<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- Username (Read-only) -->
    <div>
      <label for="username" class="block text-sm font-medium text-gray-700">Username</label>
      <input id="username" :value="props.user?.username" type="text" disabled
        class="mt-1 block w-full rounded-md border-gray-300 bg-gray-50 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm cursor-not-allowed" />
    </div>
    <div class="grid grid-cols-2 gap-4">
      <!-- First Name -->
      <div>
        <label for="firstName" class="block text-sm font-medium text-gray-700">First Name</label>
        <input id="firstName" v-model="form.firstName" type="text" required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
      </div>

      <!-- Last Name -->
      <div>
        <label for="lastName" class="block text-sm font-medium text-gray-700">Last Name</label>
        <input id="lastName" v-model="form.lastName" type="text" required
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
      </div>
    </div>

    <!-- Email -->
    <div>
      <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
      <input id="email" v-model="form.email" type="email" required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
    </div>

    <!-- Password Change Section -->
    <div class="pt-6 border-t">
      <h3 class="text-lg font-medium text-gray-900 mb-4">Change Password</h3>
      <div class="space-y-4">
        <div>
          <label for="currentPassword" class="block text-sm font-medium text-gray-700">Current Password</label>
          <input id="currentPassword" v-model="passwordForm.currentPassword" type="password"
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
        </div>
        <div>
          <label for="newPassword" class="block text-sm font-medium text-gray-700">New Password</label>
          <input id="newPassword" v-model="passwordForm.newPassword" type="password"
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
        </div>
        <div>
          <label for="confirmPassword" class="block text-sm font-medium text-gray-700">Confirm Password</label>
          <input id="confirmPassword" v-model="passwordForm.confirmPassword" type="password"
            class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
        </div>
      </div>
    </div>

    <!-- TOTP Setup Section - Only for Students -->
    <div v-if="isStudent" class="mt-6">
      <h3 class="text-lg font-medium">Two-Factor Authentication</h3>
      <div v-if="!user?.isTwoFactorEnabled" class="mt-2">
        <Button @click="setupTotp">Setup 2FA</Button>
      </div>
      <div v-else-if="!user?.isTwoFactorVerified" class="mt-2 text-yellow-600">
        2FA is set up but not verified
      </div>
      <div v-else class="mt-2 text-green-600 flex items-center">
        <CheckCircleIcon class="h-5 w-5 mr-2" />
        2FA is enabled and verified
      </div>
    </div>

    <!-- TOTP Setup Modal -->
    <Modal v-model="showTotpModal" title="Set up Two-Factor Authentication">
      <template #default>
        <div v-if="totpSetupData" class="space-y-6">
          <!-- Step 1: Show QR Code -->
          <div v-if="!isVerifyingTotp" class="space-y-6">
            <div class="text-center">
              <img :src="qrCodeDataUrl" alt="QR Code" class="mx-auto w-48 h-48" />
              <p class="mt-2 text-sm text-gray-600">
                Scan this QR code with your authenticator app
              </p>
            </div>

            <div class="mt-4">
              <p class="text-sm font-medium">Manual Entry Code:</p>
              <code class="block p-2 mt-1 bg-gray-100 rounded text-sm break-all">
              {{ totpSetupData.secretKey }}
            </code>
            </div>

            <div class="mt-6">
              <Button @click="startVerification" class="w-full">
                I've saved these details
              </Button>
            </div>
          </div>

          <!-- Step 2: Verify Code -->
          <div v-else class="space-y-6">
            <div class="text-center">
              <h3 class="text-lg font-medium">Verify Setup</h3>
              <p class="mt-2 text-sm text-gray-600">
                Enter the code from your authenticator app to verify the setup
              </p>
            </div>

            <div>
              <label for="verificationCode" class="block text-sm font-medium text-gray-700">
                Verification Code
              </label>
              <input id="verificationCode" v-model="verificationCode" type="text" maxlength="6" pattern="\d{6}"
                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm text-center tracking-widest font-mono"
                placeholder="000000" @input="handleCodeInput" />
            </div>

            <div v-if="verificationError" class="text-sm text-red-600 text-center">
              {{ verificationError }}
            </div>

            <div class="flex space-x-3">
              <Button @click="cancelVerification" variant="secondary" class="flex-1">
                Back
              </Button>
              <Button @click="confirmTotpSetup" :disabled="!isValidCode" :loading="isVerifying" class="flex-1">
                Verify
              </Button>
            </div>
          </div>
        </div>
      </template>
    </Modal>

    <div v-if="error" class="text-sm text-red-600">
      {{ error }}
    </div>

    <div class="flex justify-end space-x-3">
      <Button type="submit" :loading="isLoading">Save Changes</Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import { CheckCircleIcon } from '@heroicons/vue/24/solid'
import Button from '@/shared/components/ui/Button.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import * as QRCode from 'qrcode'
import type { QRCodeToDataURLOptions } from 'qrcode'
import type {
  UserDto,
  UpdateProfileCommand,
  ChangePasswordCommand,
  TotpSetupDto
} from '@/api/generated/data-contracts'

const isStudent = computed(() =>
  props.user?.role === 4 // Student role
)

const showTotpModal = ref(false)
const totpSetupData = ref<TotpSetupDto | null>(null)
const qrCodeDataUrl = ref<string>('')
const isVerifyingTotp = ref(false)
const verificationCode = ref('')
const verificationError = ref('')
const isVerifying = ref(false)

const props = defineProps<{
  user: UserDto | null;
  totpSetup?: TotpSetupDto | null;
}>()

const isValidCode = computed(() => {
  return /^\d{6}$/.test(verificationCode.value)
})

const emit = defineEmits<{
  (e: 'updateProfile', data: UpdateProfileCommand): void
  (e: 'updatePassword', data: ChangePasswordCommand): void
  (e: 'setupTotp'): void
  (e: 'confirmTotp', code: string): void
}>()

function setupTotp() {
  emit('setupTotp')
}

function handleCodeInput(event: Event) {
  const input = event.target as HTMLInputElement
  // Only allow numbers
  input.value = input.value.replace(/\D/g, '')
  verificationCode.value = input.value
}

function startVerification() {
  isVerifyingTotp.value = true
  verificationError.value = ''
  verificationCode.value = ''
}

function cancelVerification() {
  isVerifyingTotp.value = false
  verificationError.value = ''
  verificationCode.value = ''
}

async function confirmTotpSetup() {
  if (!isValidCode.value) return

  isVerifying.value = true
  verificationError.value = ''

  try {
    emit('confirmTotp', verificationCode.value)
    closeModal()
  } catch (err) {
    verificationError.value = 'Invalid verification code'
  } finally {
    isVerifying.value = false
  }
}

function closeModal() {
  showTotpModal.value = false
  totpSetupData.value = null
  isVerifyingTotp.value = false
  verificationCode.value = ''
  verificationError.value = ''
}

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

watch(() => props.totpSetup, async (newSetup) => {
  if (newSetup) {
    totpSetupData.value = newSetup
    showTotpModal.value = true

    try {
      // Update QR code generation with proper typing
      const options: QRCodeToDataURLOptions = {
        width: 200,
        margin: 2,
        color: {
          dark: '#000000',
          light: '#ffffff'
        }
      }

      qrCodeDataUrl.value = await QRCode.toDataURL(
        newSetup.qrCodeUri || '',
        options
      )
    } catch (err) {
      console.error('Failed to generate QR code:', err)
      error.value = 'Failed to generate QR code'
    }
  }
})

async function handleSubmit() {
  try {
    isLoading.value = true
    error.value = ''

    // Track what needs to be updated
    const updates: { profile?: boolean; password?: boolean } = {};

    // Check if profile details have changed
    if (form.value.firstName !== props.user?.firstName ||
      form.value.lastName !== props.user?.lastName ||
      form.value.email !== props.user?.email) {
      updates.profile = true;
    }

    // Check if password change is attempted
    if (passwordForm.value.newPassword || passwordForm.value.currentPassword || passwordForm.value.confirmPassword) {
      if (!passwordForm.value.currentPassword) {
        error.value = 'Current password is required to change password'
        return
      }
      if (passwordForm.value.newPassword !== passwordForm.value.confirmPassword) {
        error.value = 'New passwords do not match'
        return
      }
      updates.password = true;
    }

    // Emit appropriate events
    if (updates.profile) {
      emit('updateProfile', form.value)
    }

    if (updates.password) {
      emit('updatePassword', {
        currentPassword: passwordForm.value.currentPassword,
        newPassword: passwordForm.value.newPassword
      })
    }

  } finally {
    isLoading.value = false
  }
}
</script>

<style scoped>
input[type="text"][pattern="\d{6}"] {
  letter-spacing: 0.5em;
}
</style>