import type { User } from './user.types';

export interface Professor extends User {
  departmentId: number;
  departmentName?: string;
  specialization?: string;
}

export interface Course {
  id: number;
  name: string;
  code: string;
  description?: string;
  departmentId: number;
  departmentName?: string;
}

export interface StudyGroup {
  id: number;
  name: string;
  subjectId: number;
  subjectName: string;
  academicYearId: number;
  academicYearName: string;
  studentsCount: number;
}

export interface ClassSchedule {
  id: number;
  groupId: number;
  groupName: string;
  classroomId: number;
  classroomName: string;
  dayOfWeek: number;
  startTime: string;
  endTime: string;
  isActive: boolean;
}

export interface AttendanceReport {
  groupId: number;
  groupName: string;
  totalClasses: number;
  totalStudents: number;
  averageAttendance: number;
  absenceAlerts: number;
  details: AttendanceReportDetail[];
}

export interface AttendanceReportDetail {
  studentId: number;
  studentName: string;
  attendedClasses: number;
  absencePercentage: number;
  lastAttendance?: Date;
}