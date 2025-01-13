import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  AuthResponse, 
  LoginRequest,
  RegisterRequest,
  RefreshTokenRequest,
  ResetPasswordRequest 
} from '@/api/generated/data-contracts';
import { NumericRole, getRoleString } from '@/types/base.types';
import { Auth } from '@/api/generated/Auth';
import type { 
  LoginCommand, 
  RefreshTokenCommand,
  ResetPasswordCommand 
} from '@/api/generated/data-contracts';

const authApi = new Auth();

export const useAuthStore = defineStore('auth', () => {
  // State
  const user = ref<User | null>(JSON.parse(localStorage.getItem('user') || 'null'));
  const token = ref<string | null>(localStorage.getItem('token'));
  const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'));
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const isAuthenticated = computed(() => !!token.value);
  const userRole = computed(() => user.value?.role.toLowerCase());
  const fullName = computed(() => 
    user.value ? `${user.value.firstName} ${user.value.lastName}` : ''
  );

  // Actions
  async function login(credentials: LoginRequest) {
    isLoading.value = true;
    error.value = null;
    
    try {
      const { data } = await authApi.authLoginCreate(credentials as LoginCommand);
      setAuthData(data);
    } catch (err: any) {
      console.error('Login error:', err);
      error.value = err?.response?.data?.message || 'Failed to sign in';
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function register(userData: RegisterRequest) {
    isLoading.value = true;
    try {
      const { data } = await authApi.authRegisterCreate(userData);
      setAuthData(data);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function logout() {
    if (token.value) {
      try {
        await authApi.authLogoutCreate();
      } catch (err) {
        console.error('Logout error:', err);
      }
    }
    clearAuthData();
  }

  async function refreshTokens() {
    if (!token.value || !refreshToken.value) {
      throw new Error('No refresh token available');
    }

    try {
      const { data } = await authApi.authRefreshTokenCreate({
        accessToken: token.value,
        refreshToken: refreshToken.value
      } as RefreshTokenCommand);
      
      setAuthData(data);
      return data;
    } catch (err) {
      clearAuthData();
      throw err;
    }
  }

  async function resetPassword(request: ResetPasswordRequest) {
    isLoading.value = true;
    try {
      await authApi.authResetPasswordCreate(request as ResetPasswordCommand);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  // Helper functions
  function setAuthData(data: AuthResponse) {
    if (!data.user || !data.accessToken || !data.refreshToken) {
      console.error('Invalid auth response:', data);
      throw new Error('Invalid authentication response');
    }

    const mappedRole = getRoleString(data.user.role as NumericRole);
    const userData = {
      ...data.user,
      role: mappedRole
    };

    user.value = userData;
    token.value = data.accessToken;
    refreshToken.value = data.refreshToken;
    
    localStorage.setItem('token', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    localStorage.setItem('user', JSON.stringify(userData));
  }

  function clearAuthData() {
    user.value = null;
    token.value = null;
    refreshToken.value = null;
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken'); 
    localStorage.removeItem('user');
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
    logout,
    refreshTokens,
    resetPassword
  };
});