import type { BaseEntity, NumericRole, StringRole } from './base.types';

// Core User interface
export interface User extends BaseEntity {
  username: string;
  email: string;
  role: NumericRole | StringRole;
  firstName: string;
  lastName: string;
  isActive: boolean;
}

// Auth Response Types
export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: {
    id: number;
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    role: number; 
  };
}

// Auth Request Types
export interface LoginRequest {
  username: string;
  password: string;
  rememberMe?: boolean;
}

export interface RegisterRequest {
  username: string;
  password: string;
  email: string;
  firstName: string;
  lastName: string;
  role: Role;
  studentId?: string;
  departmentId?: number;
  cardId?: string;
}

export interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
  confirmPassword: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

// Password Reset Flow Types
export interface RequestPasswordResetRequest {
  email: string;
}

export interface RequestPasswordResetResponse {
  message: string;
  emailSent: boolean;
}

export interface VerifyTokenRequest {
  token: string;
}

export interface VerifyTokenResponse {
  isValid: boolean;
  email?: string;
}

// Error Response Type
export interface AuthError {
  message: string;
  errors?: Record<string, string[]>;
}