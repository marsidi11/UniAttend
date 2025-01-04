import apiClient from '../apiClient';
import type { 
  StudentProfile, 
  AttendanceRecord,
  AbsenceAlert,
  StudentStats,
  StudentGroupDetails,
  UpdateStudentRequest
} from '@/types/student.types';

export const studentApi = {
  getProfile: () =>
    apiClient.get<StudentProfile>('/student/profile'),

  updateProfile: (data: UpdateStudentRequest) =>
    apiClient.put<StudentProfile>('/student/profile', data),

  getAttendanceHistory: (startDate?: Date, endDate?: Date) =>
    apiClient.get<AttendanceRecord[]>('/student/attendance', {
      params: { startDate, endDate }
    }),

  getAbsencePercentage: (groupId: number) =>
    apiClient.get<number>(`/student/absence-percentage/${groupId}`),

  getAbsenceAlerts: () =>
    apiClient.get<AbsenceAlert[]>('/student/absence-alerts'),

  getDashboardStats: () =>
    apiClient.get<StudentStats>('/student/dashboard-stats'),

  getGroupDetails: (groupId: number) =>
    apiClient.get<StudentGroupDetails>(`/student/group/${groupId}`),

  registerCard: (cardId: string) =>
    apiClient.post('/student/card', { cardId }),

  requestOtp: (classId: number) =>
    apiClient.post<{ otpCode: string }>('/student/request-otp', { classId })
};