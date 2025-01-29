import { defineStore } from 'pinia';
import { ref } from 'vue';
import { useToast } from 'vue-toastification';
import type {
  UserDto,
  UserProfileDto,
  UserDetailsDto,
  UpdateProfileCommand,
  ChangePasswordCommand,
  CreateUserCommand,
  UpdateUserCommand,
  TotpSetupDto
} from '@/api/generated/data-contracts';
import { UserRole } from '@/api/generated/data-contracts';
import { userApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useUserStore = defineStore('user', () => {
  const toast = useToast();

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
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchUserById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await userApi.userDetail(id);
      const existingIndex = users.value.findIndex(u => u.id === id);
      if (existingIndex !== -1) {
        users.value[existingIndex] = data;
      }
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createUser(userData: CreateUserCommand) {
    isLoading.value = true;
    try {
      const response = await userApi.userCreate(userData);
      // Only proceed if first call succeeds
      const { data: newUserId } = response;
      const { data: newUser } = await userApi.userDetail(newUserId);
      users.value.push(newUser);
      toast.success('User created successfully');
      return newUser;
    } catch (err) {
      // Let handleError show the validation message
      handleError(err, error);
      throw err; // Re-throw to handle in component
    } finally {
      isLoading.value = false;
    }
  }

  async function updateUser(id: number, userData: UpdateUserCommand) {
    isLoading.value = true;
    try {
      await userApi.userUpdate(id, { id, ...userData });
      const updatedUser = await userApi.userDetail(id);
      const index = users.value.findIndex(u => u.id === id);
      if (index !== -1) {
        users.value[index] = updatedUser.data;
      }
      toast.success('User updated successfully');
      return updatedUser.data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deactivateUser(id: number) {
    isLoading.value = true;
    try {
      await userApi.userDeactivateCreate(id);
      const index = users.value.findIndex(u => u.id === id);
      if (index !== -1) {
        users.value[index] = { ...users.value[index], isActive: false };
      }
      toast.success('User deactivated successfully');
    } catch (err) {
      handleError(err, error);
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
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateProfile(profileData: UpdateProfileCommand) {
    isLoading.value = true;
    try {
      await userApi.userProfileUpdate(profileData);
      const updatedProfile = await fetchProfile();
      profile.value = updatedProfile;
      toast.success('Profile updated successfully');
      return updatedProfile;
    } catch (err) {
      handleError(err, error);
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
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function changePassword(passwordData: ChangePasswordCommand) {
    isLoading.value = true;
    try {
      await userApi.userChangePasswordUpdate(passwordData);
      toast.success('Password changed successfully');
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function setupTotp(): Promise<TotpSetupDto> {
    isLoading.value = true;
    try {
      const { data } = await userApi.userSetupTotpCreate();
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function verifyTotp(code: string): Promise<boolean> {
    isLoading.value = true;
    try {
      const { data } = await userApi.userVerifyTotpCreate(code);
      if (data) {
        await fetchProfile(); // Refresh user data after successful verification
      }
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
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
    changePassword,
    setupTotp,
    verifyTotp
  };
});