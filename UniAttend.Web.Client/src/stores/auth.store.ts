import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { User, AuthResponse, LoginRequest, RegisterRequest } from '@/types/user.types';
import { authApi } from '@/api/endpoints/authApi';

export const useAuthStore = defineStore('auth', () => {
  // State
  const user = ref<User | null>(null);
  const token = ref<string | null>(localStorage.getItem('token'));
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const isAuthenticated = computed(() => !!token.value);
  const userRole = computed(() => user.value?.role.toLowerCase());
  const fullName = computed(() => 
    user.value ? `${user.value.firstName} ${user.value.lastName}` : '');

  // Actions
  async function login(credentials: LoginRequest) {
    isLoading.value = true;
    try {
      const { data } = await authApi.login(credentials);
      setAuthData(data);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function register(userData: RegisterRequest) {
    isLoading.value = true;
    try {
      const { data } = await authApi.register(userData);
      setAuthData(data);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  function logout() {
    user.value = null;
    token.value = null;
    localStorage.removeItem('token');
  }

  function setAuthData(data: AuthResponse) {
    user.value = data.user;
    token.value = data.token;
    localStorage.setItem('token', data.token);
  }

  function handleError(err: unknown) {
    error.value = err instanceof Error ? err.message : 'An error occurred';
  }

  return {
    // State
    user,
    token,
    isLoading,
    error,
    
    // Getters
    isAuthenticated,
    userRole,
    fullName,
    
    // Actions
    login,
    register,
    logout
  };
});