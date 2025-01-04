import apiClient from '../apiClient';
import type { 
  AttendanceRecord, 
  AttendanceStats,
  ClassSession,
  ClassAttendance,
  RecordCardAttendanceRequest,
  RecordOtpAttendanceRequest,
  AttendanceReport
} from '@/types/attendance.types';

export const attendanceApi = {
  // Student attendance
  getStudentAttendance: (startDate?: Date, endDate?: Date) =>
    apiClient.get<AttendanceRecord[]>('/attendance/student', { 
      params: { startDate, endDate } 
    }),

  checkInWithCard: (request: RecordCardAttendanceRequest) =>
    apiClient.post<AttendanceRecord>('/attendance/check-in/card', request),

  checkInWithOtp: (request: RecordOtpAttendanceRequest) =>
    apiClient.post<AttendanceRecord>('/attendance/check-in/otp', request),

  getStudentStats: (groupId: number) =>
    apiClient.get<AttendanceStats>(`/attendance/stats/${groupId}`),

  // Professor attendance management
  openClassSession: (data: Omit<ClassSession, 'id'>) =>
    apiClient.post<ClassSession>('/classes/open', data),

  closeClassSession: (classId: number) =>
    apiClient.post(`/classes/${classId}/close`),

  getClassAttendance: (classId: number) =>
    apiClient.get<ClassAttendance>(`/classes/${classId}/attendance`),

  confirmClassAttendance: (classId: number) =>
    apiClient.post(`/classes/${classId}/confirm`),

  generateAttendanceList: (groupId: number) =>
    apiClient.get<AttendanceRecord[]>(`/attendance/list/${groupId}`),

  getAbsenceReport: (groupId: number) =>
    apiClient.get<AttendanceReport>(`/attendance/absence-report/${groupId}`)
};