import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authApi } from '@/api/endpoints/authApi'
import type { 
  LoginCredentials, 
  RegisterCredentials, 
  ResetPasswordCredentials,
  AuthResponse, 
  UserProfile 
} from '@/types/auth.types'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<UserProfile | null>(null)
  const token = ref<string | null>(localStorage.getItem('token'))

  const isAuthenticated = computed(() => !!token.value)
  const userRole = computed(() => user.value?.role)

  async function login(credentials: LoginCredentials) {
    const { data } = await authApi.login(credentials)
    setAuthData(data)
    return true
  }

  async function register(credentials: RegisterCredentials) {
    const { data } = await authApi.register(credentials)
    setAuthData(data)
    return true
  }

  async function resetPassword(credentials: ResetPasswordCredentials) {
    await authApi.resetPassword(credentials)
    return true
  }

  async function requestPasswordReset(email: string) {
    await authApi.requestPasswordReset(email)
    return true
  }

  function setAuthData(auth: AuthResponse) {
    token.value = auth.token
    localStorage.setItem('token', auth.token)
  }

  function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('token')
  }

  return {
    user,
    token,
    isAuthenticated,
    userRole,
    login,
    register,
    resetPassword,
    requestPasswordReset,
    logout
  }
})