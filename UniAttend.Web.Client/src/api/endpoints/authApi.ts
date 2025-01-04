import apiClient from '../apiClient';
import type { 
  LoginRequest, 
  RegisterRequest, 
  AuthResponse,
  ResetPasswordRequest,
  ChangePasswordRequest,
  RefreshTokenRequest 
} from '@/types/auth.types';

export const authApi = {
  login: (credentials: LoginRequest) => 
    apiClient.post<AuthResponse>('/auth/login', credentials),
    
  register: (credentials: RegisterRequest) =>
    apiClient.post<AuthResponse>('/auth/register', credentials),
    
  logout: () => 
    apiClient.post('/auth/logout'),
    
  refreshToken: (data: RefreshTokenRequest) => 
    apiClient.post<AuthResponse>('/auth/refresh-token', data),
    
  changePassword: (data: ChangePasswordRequest) =>
    apiClient.post('/auth/change-password', data),
    
  requestPasswordReset: (email: string) =>
    apiClient.post('/auth/request-reset', { email }),
    
  resetPassword: (data: ResetPasswordRequest) =>
    apiClient.post('/auth/reset-password', data),
    
  verifyToken: (token: string) =>
    apiClient.post('/auth/verify-token', { token }),
    
  getCurrentUser: () =>
    apiClient.get<AuthResponse>('/auth/me')
};