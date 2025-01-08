import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { User } from '@/types/user.types';
import type { AuthResponse, LoginRequest } from '@/types/auth.types';
import { NumericRole, getRoleString } from '@/types/base.types';
import { authApi } from '@/api/endpoints/authApi';

export const useAuthStore = defineStore('auth', () => {
  // State with hydration from localStorage
  const user = ref<User | null>(JSON.parse(localStorage.getItem('user') || 'null'));
  const token = ref<string | null>(localStorage.getItem('token'));
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  function setAuthData(data: AuthResponse) {
    if (!data.user || !data.accessToken) {
      console.error('Invalid auth response:', data);
      throw new Error('Invalid authentication response');
    }

    const mappedRole = getRoleString(data.user.role as NumericRole);
    console.log('Role mapping:', {
      numericRole: data.user.role,
      mappedRole
    });

    const userData = {
      ...data.user,
      role: mappedRole
    };

    user.value = userData;
    token.value = data.accessToken;
    
    // Persist both token and user data
    localStorage.setItem('token', data.accessToken);
    localStorage.setItem('user', JSON.stringify(userData));
  }

  // Getters
  const isAuthenticated = computed(() => !!token.value);
  const userRole = computed(() => user.value?.role.toLowerCase());
  const fullName = computed(() => 
    user.value ? `${user.value.firstName} ${user.value.lastName}` : '');

  // Actions
  async function login(credentials: LoginRequest) {
    isLoading.value = true;
    error.value = null;
    
    try {
      const { data } = await authApi.login(credentials);
      console.log('Login response:', data); // Debug
      setAuthData(data);
      console.log('Auth state after login:', { 
        user: user.value,
        role: userRole.value,
        token: !!token.value
      }); // Debug
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
    localStorage.removeItem('user');
  }

  function setAuthData(data: AuthResponse) {
    if (!data.user || !data.accessToken) {
      console.error('Invalid auth response:', data);
      throw new Error('Invalid authentication response');
    }
  
    console.log('Setting auth data:', {
      originalRole: data.user.role,
      originalRoleType: typeof data.user.role,
      token: !!data.accessToken
    });
  
    const mappedRole = getRoleString(data.user.role as NumericRole);
    
    user.value = {
      ...data.user,
      role: mappedRole
    };
    token.value = data.accessToken;
    
    localStorage.setItem('token', data.accessToken);
    localStorage.setItem('user', JSON.stringify(user.value));
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