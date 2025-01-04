import type { BaseEntity } from './base.types';

export interface OtpCode extends BaseEntity {
  studentId: number;
  classId: number;
  code: string;
  expiryTime: Date;
  isUsed: boolean;
}