import apiClient  from '../apiClient';
import type { 
  User,
  UserProfile, 
  UserDetails,
  UpdateProfileRequest, 
  ChangePasswordRequest 
} from '@/types/user.types';

export const userApi = {
  // Get current user profile
  getAll: () =>
    apiClient.get<User[]>('/user'),
  
  getProfile: () =>
    apiClient.get<UserProfile>('/user/profile'),

  // Get detailed user information
  getUserDetails: () =>
    apiClient.get<UserDetails>('/user/details'),

  // Update user profile
  updateProfile: (data: UpdateProfileRequest) =>
    apiClient.put('/user/profile', data),

  // Change user password
  changePassword: (data: ChangePasswordRequest) =>
    apiClient.put('/user/change-password', data)
};