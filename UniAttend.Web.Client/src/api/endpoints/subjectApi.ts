import apiClient from '../apiClient';
import type { 
  Subject,
  SubjectDetails,
} from '@/types/subject.types';
import type { StudyGroup } from '@/types/group.types';

export const subjectApi = {
  // Get all subjects with optional department filter
  getAll: (departmentId?: number) =>
    apiClient.get<Subject[]>('/subjects', { 
      params: { departmentId } 
    }),

  // Get subject by ID with expanded details
  getById: (id: number) =>
    apiClient.get<SubjectDetails>(`/subjects/${id}`),

  // Create new subject
  create: (data: Partial<Subject>) =>
    apiClient.post<Subject>('/subjects', data),

  // Update existing subject
  update: (id: number, data: Partial<Subject>) =>
    apiClient.put<Subject>(`/subjects/${id}`, data),

  // Delete subject (soft delete)
  delete: (id: number) =>
    apiClient.delete(`/subjects/${id}`),

  // Get study groups for a subject
  getGroups: (subjectId: number) =>
    apiClient.get<StudyGroup[]>(`/subjects/${subjectId}/groups`),

  // Get active subjects by department
  getActiveByDepartment: (departmentId: number) =>
    apiClient.get<Subject[]>(`/subjects/department/${departmentId}/active`),

  // Get subject statistics
  getStats: (subjectId: number) =>
    apiClient.get<{
      groupsCount: number;
      studentsCount: number;
      averageAttendance: number;
    }>(`/subjects/${subjectId}/stats`),

  // Check if subject name exists in department
  checkNameExists: (name: string, departmentId: number) =>
    apiClient.get<boolean>('/subjects/check-name', {
      params: { name, departmentId }
    })
};