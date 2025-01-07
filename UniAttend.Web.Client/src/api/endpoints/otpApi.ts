import apiClient from '../apiClient';
import type { OtpCode, OtpValidationResponse } from '@/types/otp.types';

export const otpApi = {
  // Generate OTP code for a class session
  generate: (classId: number) =>
    apiClient.post<OtpCode>('/attendance/otp/generate', { classId }),

  // Validate OTP code
  validate: (code: string, classId: number) =>
    apiClient.post<OtpValidationResponse>('/attendance/otp/validate', { 
      code, 
      classId 
    }),

  // Request new OTP code (student)
  requestOtp: (classId: number) =>
    apiClient.post<OtpCode>('/attendance/otp/request', { classId }),

  // Get current active OTP for class (professor)
  getCurrentOtp: (classId: number) =>
    apiClient.get<OtpCode>(`/attendance/otp/${classId}/current`)
};