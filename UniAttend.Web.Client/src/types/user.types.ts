import type { BaseEntity, Role } from './base.types';

// Core User Interface
export interface User extends BaseEntity {
  id: number;
  username: string;
  email: string;
  role: Role;
  firstName: string;
  lastName: string;
  isActive: boolean;
  departmentId?: number;
}

// User Profile 
export interface UserProfile {
  firstName: string;
  lastName: string;
  email: string;
  username: string;
}

// User Details
export interface UserDetails extends UserProfile {
  role: Role;
  departmentName?: string;
  attendanceStats?: {
    totalClasses: number;
    attendedClasses: number;
    attendanceRate: number;
  };
}

// Request Types
export interface UpdateProfileRequest {
  firstName: string;
  lastName: string;
  email: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}