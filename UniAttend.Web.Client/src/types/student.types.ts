import type { User } from './user.types';

export interface Student extends User {
  studentId: string;
  cardId?: string;
  departmentId: number;
  departmentName?: string;
}

export interface StudentAttendance {
  courseId: number;
  courseName: string;
  checkInTime: Date;
  checkInMethod: 'Card' | 'OTP';
  isConfirmed: boolean;
}

export interface StudentGroup {
  groupId: number;
  groupName: string;
  subjectName: string;
  professorName: string;
  academicYear: string;
}