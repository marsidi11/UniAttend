import type { BaseEntity, Role } from './base.types';
import type { User } from './user.types';
import type { Department } from './department.types';

// Staff Management
export interface CreateStaffCommand {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  role: Role;
  departmentId?: number;
  password?: string;
  isActive: boolean;
}

export interface UpdateStaffRequest {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  departmentId?: number;
  isActive: boolean;
}

export interface SystemStats {
  totalUsers: number;
  activeUsers: number;
  totalStudents: number;
  totalProfessors: number;
  totalDepartments: number;
  totalGroups: number;
  averageAttendance: number;
  activeClasses: number;
}

export interface AcademicYear extends BaseEntity {
  name: string;
  startDate: Date;
  endDate: Date;
  isActive: boolean;
}

export interface Subject extends BaseEntity {
  name: string;
  departmentId: number;
  departmentName?: string;
  isActive: boolean;
}

export interface AdminAttendanceReport {
  startDate: Date;
  endDate: Date;
  departmentId?: number;
  departmentName?: string;
  totalStudents: number;
  totalClasses: number;
  averageAttendance: number;
  departments: DepartmentAttendance[];
}

export interface DepartmentAttendance {
  departmentId: number;
  departmentName: string;
  studentCount: number;
  attendanceRate: number;
  groups: GroupAttendance[];
}

export interface GroupAttendance {
  groupId: number;
  groupName: string;
  subjectName: string;
  enrolledStudents: number;
  attendanceRate: number;
}

export interface SystemLog {
  timestamp: Date;
  level: 'Info' | 'Warning' | 'Error';
  message: string;
  source: string;
  details?: string;
}

export interface BackupInfo {
  id: number;
  filename: string;
  createdAt: Date;
  size: number;
  status: 'Completed' | 'Failed';
}