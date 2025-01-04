import type { BaseEntity } from './base.types';

export interface AttendanceRecord extends BaseEntity {
  studentId: number;
  classId: number;
  checkInTime: Date;
  checkInMethod: 'Card' | 'OTP';
  isConfirmed: boolean;
  studentName?: string;
  courseName?: string;
  professorName?: string;
}

export interface AttendanceStats {
  totalStudents: number;
  presentToday: number;
  attendanceRate: number;
  absentStudents: number;
}

export interface ClassSession extends BaseEntity {
  groupId: number;
  classroomId: number;
  date: Date;
  startTime: string;
  endTime: string;
  status: 'Active' | 'Completed';
}

export interface ClassAttendance extends ClassSession {
  records: AttendanceRecord[];
  stats: AttendanceStats;
}

export interface RecordCardAttendanceRequest {
  cardId: string;
  deviceId: string;
  classId: number;
}

export interface RecordOtpAttendanceRequest {
  otpCode: string;
  classId: number;
  studentId?: number;
}

export interface AttendanceReport {
  groupId: number;
  totalClasses: number;
  attendedClasses: number;
  absenceRate: number;
  records: AttendanceRecord[];
}