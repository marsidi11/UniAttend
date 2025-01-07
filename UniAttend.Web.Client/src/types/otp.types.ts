import type { BaseEntity } from './base.types';

export interface OtpCode extends BaseEntity {
  code: string;
  classId: number;
  studentId: number;
  expiryTime: Date;
  isUsed: boolean;
}

export interface OtpValidationResponse {
  isValid: boolean;
  message: string;
  attendanceRecord?: {
    id: number;
    checkInTime: Date;
    status: 'pending' | 'confirmed';
  };
}

export interface GenerateOtpRequest {
  classId: number;
}

export interface ValidateOtpRequest {
  code: string;
  classId: number;
  studentId: number;
}