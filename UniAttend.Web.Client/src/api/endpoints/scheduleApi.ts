import apiClient from '../apiClient';
import type { 
  Schedule, 
  CreateScheduleRequest,
  UpdateScheduleRequest,
  ScheduleConflictRequest 
} from '@/types/schedule.types';

export const scheduleApi = {
  // Get all schedules with optional filters
  getAll: (params?: { groupId?: number; classroomId?: number }) =>
    apiClient.get<Schedule[]>('/schedule', { params }),

  // Get schedule by ID
  getById: (id: number) =>
    apiClient.get<Schedule>(`/schedule/${id}`),

  // Create new schedule
  create: (data: CreateScheduleRequest) =>
    apiClient.post<Schedule>('/schedule', data),

  // Update schedule
  update: (id: number, data: UpdateScheduleRequest) =>
    apiClient.put<Schedule>(`/schedule/${id}`, data),

  // Delete schedule
  delete: (id: number) =>
    apiClient.delete(`/schedule/${id}`),

  // Get schedule by group
  getByGroup: (groupId: number) =>
    apiClient.get<Schedule[]>(`/schedule/group/${groupId}`),

  // Get schedule by classroom
  getByClassroom: (classroomId: number) =>
    apiClient.get<Schedule[]>(`/schedule/classroom/${classroomId}`),

  // Check for schedule conflicts
  checkConflict: (request: ScheduleConflictRequest) =>
    apiClient.post<boolean>('/schedule/check-conflict', request),

  // Get weekly schedule
  getWeeklySchedule: (params: { groupId?: number; classroomId?: number }) =>
    apiClient.get<Record<number, Schedule[]>>('/schedule/weekly', { params })
};