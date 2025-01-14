import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { 
  UserDto,
  UserProfileDto, 
  UserDetailsDto, 
  UpdateProfileCommand, 
  ChangePasswordCommand,
  CreateUserCommand,
  UpdateUserCommand
} from '@/api/generated/data-contracts';
import { UserRole } from '@/api/generated/data-contracts';
import { userApi } from '@/api/apiInstances';

export const useUserStore = defineStore('user', () => {
  // State
  const users = ref<UserDto[]>([]);
  const profile = ref<UserProfileDto | null>(null);
  const details = ref<UserDetailsDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Actions
  async function fetchUsers(filters?: { role?: UserRole; isActive?: boolean }) {
    isLoading.value = true;
    try {
      const { data } = await userApi.userList(filters);
      users.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchUserById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await userApi.userDetail(id);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createUser(userData: CreateUserCommand) {
    isLoading.value = true;
    try {
      const { data } = await userApi.userCreate(userData);
      await fetchUsers(); // Refresh users list
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateUser(id: number, userData: UpdateUserCommand) {
    isLoading.value = true;
    try {
      await userApi.userUpdate(id, userData);
      await fetchUsers(); // Refresh users list
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deactivateUser(id: number) {
    isLoading.value = true;
    try {
      await userApi.userDeactivateCreate(id);
      await fetchUsers(); // Refresh users list
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchProfile() {
    isLoading.value = true;
    try {
      const { data } = await userApi.userProfileList();
      profile.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateProfile(profileData: UpdateProfileCommand) {
    isLoading.value = true;
    try {
      await userApi.userProfileUpdate(profileData);
      await fetchProfile(); // Refresh profile data
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchUserDetails(id?: number) {
    isLoading.value = true;
    try {
      const { data } = id 
        ? await userApi.userDetailsDetail(id)
        : await userApi.userDetailsList();
      details.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function changePassword(passwordData: ChangePasswordCommand) {
    isLoading.value = true;
    try {
      await userApi.userChangePasswordUpdate(passwordData);
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
    users,
    profile,
    details,
    isLoading,
    error,

    // Actions
    fetchUsers,
    fetchUserById,
    createUser,
    updateUser,
    deactivateUser,
    fetchProfile,
    updateProfile,
    fetchUserDetails,
    changePassword
  };
});