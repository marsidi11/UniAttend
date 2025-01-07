import apiClient from '../apiClient';
import type { 
  Professor,
  StudyGroup, 
  ClassSchedule,
  AttendanceReport,
  Course
} from '@/types/professor.types';

export const professorApi = {
  getAll: (departmentId?: number) =>
    apiClient.get<Professor[]>('/professor', { 
      params: { departmentId } 
    }),

  getById: (id: number) =>
    apiClient.get<Professor>(`/professor/${id}`),

  create: (data: Partial<Professor>) =>
    apiClient.post<Professor>('/professor', data),

  update: (id: number, data: Partial<Professor>) =>
    apiClient.put<Professor>(`/professor/${id}`, data),

  getAssignedGroups: () =>
    apiClient.get<StudyGroup[]>('/professor/groups'),

  getTodayClasses: () =>
    apiClient.get<ClassSchedule[]>('/professor/classes/today'),

  getSchedule: (weekStart?: Date) =>
    apiClient.get<ClassSchedule[]>('/professor/schedule', { 
      params: { weekStart } 
    }),

  getAssignedCourses: (professorId: number) =>
    apiClient.get<Course[]>(`/professor/${professorId}/courses`),

  generateGroupReport: (groupId: number) =>
    apiClient.get<AttendanceReport>(`/professor/reports/${groupId}`),

  getAbsenceAlerts: (groupId: number) =>
    apiClient.get<AttendanceReport>(`/professor/absence-alerts/${groupId}`),

  confirmAttendance: (classId: number) =>
    apiClient.post(`/professor/classes/${classId}/confirm`),

  openClassSession: (groupId: number, classroomId: number) =>
    apiClient.post('/professor/classes/open', { groupId, classroomId }),

  closeClassSession: (classId: number) =>
    apiClient.post(`/professor/classes/${classId}/close`)
};