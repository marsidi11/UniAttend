import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  LoginCommand,
  ResetPasswordCommand,
  UserDto,
  AuthResult,
  UserRole
} from '@/api/generated/data-contracts';
import { NumericRole, StringRole, getRoleString } from '@/types/base.types';
import { Auth } from '@/api/generated/Auth';
import { authApi } from '@/api/apiInstances';

type LoginRequest = LoginCommand;
type ResetPasswordRequest = ResetPasswordCommand;
type ExtendedUserDto = Omit<UserDto, 'role'> & { role: StringRole };

export const useAuthStore = defineStore('auth', () => {
  // State
  const user = ref<ExtendedUserDto | null>(JSON.parse(localStorage.getItem('user') || 'null'));
  const token = ref<string | null>(localStorage.getItem('token'));
  const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'));
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const isAuthenticated = computed(() => !!token.value);
  const userRole = computed(() => user.value?.role);
  const fullName = computed(() => 
    user.value ? `${user.value.firstName} ${user.value.lastName}` : ''
  );

  // Actions
  async function login(credentials: LoginRequest) {
    isLoading.value = true;
    error.value = null;
    
    try {
      const { data } = await authApi.authLoginCreate(credentials);
      setAuthData(data);
    } catch (err: any) {
      console.error('Login error:', err);
      error.value = err?.response?.data?.message || 'Failed to sign in';
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
      });
  
      if (!data) {
        throw new Error('No data received from refresh token request');
      }
  
      // Update tokens and user data
      setAuthData(data);
      return data;
    } catch (error) {
      console.error('Token refresh failed:', error);
      clearAuthData();
      throw error;
    }
  }

  async function resetPassword(request: ResetPasswordRequest) {
    isLoading.value = true;
    try {
      await authApi.authResetPasswordCreate(request);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  function setAuthData(data: AuthResult) {
    if (!data.user || !data.accessToken || !data.refreshToken) {
      console.error('Invalid auth response:', data);
      throw new Error('Invalid authentication response');
    }

    const mappedRole = getRoleString(data.user.role as unknown as NumericRole);
    const userData: ExtendedUserDto = {
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
    logout,
    refreshTokens,
    resetPassword
  };
});