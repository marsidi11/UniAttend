import type { BaseEntity } from './base.types';

export interface AbsenceAlert extends BaseEntity {
  studentId: number;
  groupId: number;
  subjectName: string; 
  absencePercentage: number;
  emailSent: boolean;
  createdAt: Date;
  student?: {
    firstName: string;
    lastName: string;
    email: string;
  };
  group?: {
    name: string;
    subjectName: string;
  };
}

export interface AlertSummary {
  totalAlerts: number;
  unsentAlerts: number;
  criticalAlerts: number; // alerts > 25% absence
  latestAlerts: AbsenceAlert[];
}

export interface MarkAlertSentRequest {
  alertId: number;
  notificationSent: boolean;
}