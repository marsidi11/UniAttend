import apiClient from '../apiClient';
import type { AttendanceRecord, AttendanceStats } from '@/types/attendance.types';

export const attendanceApi = {
  getStudentAttendance: (startDate?: Date, endDate?: Date) =>
    apiClient.get<AttendanceRecord[]>('/attendance', { params: { startDate, endDate } }),

  getAttendanceStats: (groupId: number) =>
    apiClient.get<AttendanceStats>(`/attendance/stats/${groupId}`),

  recordCardAttendance: (cardId: string, deviceId: string) =>
    apiClient.post('/attendance/card', { cardId, deviceId }),

  recordOtpAttendance: (otpCode: string, classId: number) =>
    apiClient.post('/attendance/otp', { otpCode, classId }),

  confirmAttendance: (classId: number) =>
    apiClient.post(`/classes/${classId}/confirm`),

  getClassAttendance: (classId: number) =>
    apiClient.get<AttendanceRecord[]>(`/classes/${classId}/attendance`),

  confirmClassAttendance: (classId: number) =>
    apiClient.post(`/classes/${classId}/attendance/confirm`)
};