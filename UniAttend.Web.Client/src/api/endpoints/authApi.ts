import apiClient from '../apiClient';
import type { 
  LoginCredentials, 
  RegisterCredentials, 
  ResetPasswordCredentials, 
  AuthResponse 
} from '@/types/auth.types';

export const authApi = {
  login: (credentials: LoginCredentials) => 
    apiClient.post<AuthResponse>('/auth/login', credentials),
    
  register: (credentials: RegisterCredentials) =>
    apiClient.post<AuthResponse>('/auth/register', credentials),
    
  resetPassword: (credentials: ResetPasswordCredentials) =>
    apiClient.post('/auth/reset-password', credentials),
    
  requestPasswordReset: (email: string) =>
    apiClient.post('/auth/request-reset', { email }),

  refreshToken: (token: string) => 
    apiClient.post<AuthResponse>('/auth/refresh-token', { token }),
    
  logout: () => apiClient.post('/auth/logout')
};