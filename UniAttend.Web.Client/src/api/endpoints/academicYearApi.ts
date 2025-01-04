import apiClient from '../apiClient';
import type { 
  AcademicYear,
  AcademicYearResponse,
  CreateAcademicYearRequest,
  UpdateAcademicYearRequest 
} from '@/types/academicYear.types';

export const academicYearApi = {
  // Get all academic years
  getAll: () => 
    apiClient.get<AcademicYear[]>('/academic-years'),

  // Get active academic years
  getActive: () =>
    apiClient.get<AcademicYear[]>('/academic-years/active'),

  // Get current academic year
  getCurrent: () =>
    apiClient.get<AcademicYearResponse>('/academic-years/current'),

  // Get academic year by ID
  getById: (id: number) =>
    apiClient.get<AcademicYearResponse>(`/academic-years/${id}`),

  // Create new academic year
  create: (data: CreateAcademicYearRequest) =>
    apiClient.post<AcademicYear>('/academic-years', data),

  // Update academic year
  update: (id: number, data: UpdateAcademicYearRequest) =>
    apiClient.put<AcademicYear>(`/academic-years/${id}`, data),

  // Delete academic year
  delete: (id: number) =>
    apiClient.delete(`/academic-years/${id}`),

  // Check for date overlap
  checkOverlap: (startDate: Date, endDate: Date, excludeId?: number) =>
    apiClient.get<boolean>('/academic-years/check-overlap', {
      params: { startDate, endDate, excludeId }
    }),

  // Get groups in academic year
  getGroups: (id: number) =>
    apiClient.get<StudyGroup[]>(`/academic-years/${id}/groups`)
};