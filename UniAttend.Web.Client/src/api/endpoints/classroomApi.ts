import apiClient from '../apiClient';
import type { 
  Classroom, 
  ReaderDevice,
  ClassroomSchedule,
  ReaderDeviceStatus 
} from '@/types/classroom.types';

export const classroomApi = {
  // Classroom management
  getAll: () =>
    apiClient.get<Classroom[]>('/classrooms'),

  getById: (id: number) =>
    apiClient.get<Classroom>(`/classrooms/${id}`),

  create: (data: Omit<Classroom, 'id'>) =>
    apiClient.post<Classroom>('/classrooms', data),

  update: (id: number, data: Partial<Classroom>) =>
    apiClient.put<Classroom>(`/classrooms/${id}`, data),

  delete: (id: number) =>
    apiClient.delete(`/classrooms/${id}`),

  // Reader device management
  getActiveReaders: () =>
    apiClient.get<ReaderDevice[]>('/classrooms/readers/active'),

  getAllReaders: () =>
    apiClient.get<ReaderDevice[]>('/classrooms/readers'),

  assignReader: (classroomId: number, readerId: string) =>
    apiClient.post(`/classrooms/${classroomId}/reader`, { readerId }),
    
  removeReader: (classroomId: number) =>
    apiClient.delete(`/classrooms/${classroomId}/reader`),

  getReaderStatus: (deviceId: string) =>
    apiClient.get<ReaderDeviceStatus>(`/classrooms/readers/${deviceId}/status`),

  updateReaderStatus: (deviceId: string, status: 'Active' | 'Inactive') =>
    apiClient.put(`/classrooms/readers/${deviceId}/status`, { status }),

  // Schedule
  getSchedule: (classroomId: number, date?: Date) =>
    apiClient.get<ClassroomSchedule[]>(`/classrooms/${classroomId}/schedule`, {
      params: { date }
    }),

  checkAvailability: (classroomId: number, startTime: Date, endTime: Date) =>
    apiClient.get<boolean>(`/classrooms/${classroomId}/availability`, {
      params: { startTime, endTime }
    })
};