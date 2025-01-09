import apiClient from '../apiClient';
import type { 
  CreateStaffCommand, 
  User,
  Department,
  AcademicYear,
  Subject,
  SystemStats,
  AttendanceReport
} from '@/types/admin.types';

export const adminApi = {
  // Staff Management
  createStaff: (command: CreateStaffCommand) =>
    apiClient.post<User>('/admin/staff', command),

  getStaffList: () =>
    apiClient.get<User[]>('/admin/staff'),

  updateStaff: (id: number, data: Partial<User>) =>
    apiClient.put<User>(`/admin/staff/${id}`, data),

  deleteStaff: (id: number) =>
    apiClient.delete(`/admin/staff/${id}`),

  // Department Management
  createDepartment: (data: { name: string }) =>
    apiClient.post<Department>('/admin/departments', data),

  updateDepartment: (id: number, data: { name: string; isActive: boolean }) =>
    apiClient.put<Department>(`/admin/departments/${id}`, data),

  // Academic Year Management
  createAcademicYear: (data: { 
    name: string;
    startDate: Date;
    endDate: Date;
  }) => apiClient.post<AcademicYear>('/admin/academic-years', data),

  // Subject Management
  createSubject: (data: {
    name: string;
    departmentId: number;
  }) => apiClient.post<Subject>('/admin/subjects', data),

  // Reports
  getSystemStats: () =>
    apiClient.get<SystemStats>('/admin/stats'),

  getAttendanceReport: (params: {
    departmentId?: number;
    startDate?: Date;
    endDate?: Date;
  }) => apiClient.get<AttendanceReport>('/admin/reports/attendance', { params }),

  // System Management
  getSystemLogs: (params?: {
    startDate?: Date;
    endDate?: Date;
    level?: 'Info' | 'Warning' | 'Error';
  }) => apiClient.get('/admin/logs', { params }),

  backupDatabase: () =>
    apiClient.post('/admin/backup'),

  getBackupList: () =>
    apiClient.get('/admin/backups')
};