import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { 
  UserProfile, 
  UserDetails, 
  UpdateProfileRequest, 
  ChangePasswordRequest 
} from '@/types/user.types';
import { userApi } from '@/api/endpoints/userApi';

export const useUserStore = defineStore('user', () => {
  // State
  const profile = ref<UserProfile | null>(null);
  const details = ref<UserDetails | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Actions
  async function fetchProfile() {
    isLoading.value = true;
    try {
      const { data } = await userApi.getProfile();
      profile.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchUserDetails() {
    isLoading.value = true;
    try {
      const { data } = await userApi.getUserDetails();
      details.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateProfile(profileData: UpdateProfileRequest) {
    isLoading.value = true;
    try {
      await userApi.updateProfile(profileData);
      // Refresh profile data after update
      await fetchProfile();
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function changePassword(passwordData: ChangePasswordRequest) {
    isLoading.value = true;
    try {
      await userApi.changePassword(passwordData);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  function handleError(err: unknown) {
    error.value = err instanceof Error ? err.message : 'An error occurred';
  }

  return {
    // State
    profile,
    details,
    isLoading,
    error,

    // Actions
    fetchProfile,
    fetchUserDetails,
    updateProfile,
    changePassword
  };
});