import apiClient from '../apiClient';
import type { StudyGroup, GroupStudent } from '@/types/group.types';

export const groupApi = {
  // Get all groups with optional academic year filter
  getAll: (academicYearId?: number) =>
    apiClient.get<StudyGroup[]>('/groups', {
      params: { academicYearId }
    }),

  // Get group by ID
  getById: (id: number) =>
    apiClient.get<StudyGroup>(`/groups/${id}`),

  // Update group
  update: (id: number, data: Partial<StudyGroup>) =>
    apiClient.put<StudyGroup>(`/groups/${id}`, data),

  // Create group
  create: (data: Partial<StudyGroup>) =>
    apiClient.post<StudyGroup>('/groups', data),

  // Get group students
  getStudents: (groupId: number) =>
    apiClient.get<GroupStudent[]>(`/groups/${groupId}/students`),

  // Add single student to group
  addStudent: (groupId: number, studentId: number) =>
    apiClient.post(`/groups/${groupId}/students`, { studentId }),

  // Remove student from group
  removeStudent: (groupId: number, studentId: number) =>
    apiClient.delete(`/groups/${groupId}/students/${studentId}`),

  // Enroll multiple students
  enrollStudents: (groupId: number, studentIds: number[]) =>
    apiClient.post(`/groups/${groupId}/students/enroll`, { studentIds }),

  // Unenroll student
  unenrollStudent: (groupId: number, studentId: number) =>
    apiClient.delete(`/groups/${groupId}/students/${studentId}/unenroll`),

  // Transfer student between groups
  transferStudent: (studentId: number, fromGroupId: number, toGroupId: number) =>
    apiClient.post('/groups/transfer-student', {
      studentId,
      fromGroupId,
      toGroupId
    }),

  // Get group attendance stats
  getAttendanceStats: (groupId: number) =>
    apiClient.get(`/groups/${groupId}/attendance-stats`),

  // Get group schedule
  getSchedule: (groupId: number) =>
    apiClient.get(`/groups/${groupId}/schedule`)
};