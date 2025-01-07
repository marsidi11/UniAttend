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
  
  getStudentAttendanceStats: () =>
    apiClient.get<AttendanceStats>('/attendance/student/stats'),

  getAttendanceStats: (groupId: number) =>
    apiClient.get<AttendanceStats>(`/attendance/stats/${groupId}`),

  recordCardAttendance: (request: RecordCardAttendanceRequest) =>
    apiClient.post<AttendanceRecord>('/attendance/check-in/card', request),

  recordOtpAttendance: (request: RecordOtpAttendanceRequest) =>
    apiClient.post<AttendanceRecord>('/attendance/check-in/otp', request),

  getTodaySessions: () =>
    apiClient.get<ClassSession[]>('/attendance/today-sessions'),
    
  getRecentRecords: () =>
    apiClient.get<AttendanceRecord[]>('/attendance/recent-records'),

  // Professor attendance management
  openClass: (data: { groupId: number, classroomId: number }) =>
    apiClient.post<ClassAttendance>('/classes/open', data),

  closeClass: (classId: number) =>
    apiClient.post(`/classes/${classId}/close`),

  getClassAttendance: (classId: number) =>
    apiClient.get<ClassAttendance>(`/classes/${classId}/attendance`),

  confirmClassAttendance: (classId: number) =>
    apiClient.post(`/classes/${classId}/confirm`),

  generateList: (groupId: number) =>
    apiClient.get<AttendanceRecord[]>(`/attendance/list/${groupId}`),

  getAbsenceReport: (groupId: number) =>
    apiClient.get<AttendanceReport>(`/attendance/absence-report/${groupId}`)
};