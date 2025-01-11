import apiClient from '../apiClient';
import type { Department } from '@/types/department.types';

export const departmentApi = {
  getAll: () => 
    apiClient.get<Department[]>('/admin/departments'),

  getById: (id: number) => 
    apiClient.get<Department>(`/departments/${id}`),

  create: (payload: { name: string }) => 
    apiClient.post<Department>('/admin/departments', payload),

  update: (id: number, payload: { name: string; isActive: boolean }) => 
    apiClient.put<Department>(`/admin/departments/${id}`, payload),

  deleteDepartment: (id: number) => 
    apiClient.delete(`/admin/departments/${id}`)
};