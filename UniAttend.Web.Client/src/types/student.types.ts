import type { BaseEntity } from './base.types';
import type { Department } from './department.types';
import type { StudyGroup } from './group.types';

// Student Profile
export interface StudentProfile extends BaseEntity {
  studentId: string;
  firstName: string;
  lastName: string;
  email: string;
  cardId?: string;
  departmentId: number;
  departmentName?: string;
  department?: Department;
  groups?: StudyGroup[];
  isActive: boolean;
}

export type Student = StudentProfile

// Attendance Record
export interface AttendanceRecord extends BaseEntity {
  studentId: number;
  classId: number;
  checkInTime: Date;
  checkInMethod: 'Card' | 'OTP';
  isConfirmed: boolean;
  studentName?: string;
  className?: string;
}

// Student Absence Alert
export interface AbsenceAlert extends BaseEntity {
  studentId: number;
  groupId: number;
  groupName?: string;
  subjectName?: string;
  absencePercentage: number;
  emailSent: boolean;
  createdAt: Date;
}

// Student Dashboard Stats
export interface StudentStats {
  totalClasses: number;
  attendedClasses: number;
  absenceRate: number;
  alertCount: number;
}

// Student Group Details
export interface StudentGroupDetails {
  groupId: number;
  groupName: string;
  subjectName: string;
  professorName: string;
  attendanceRate: number;
  totalClasses: number;
  attendedClasses: number;
}

// Update Student Request
export interface UpdateStudentRequest {
  firstName?: string;
  lastName?: string;
  email?: string;
  departmentId?: number;
  isActive?: boolean;
}