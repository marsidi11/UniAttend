import apiClient from '../apiClient';
import type { Department } from '@/stores/department.store';

export const departmentApi = {
  getDepartments: () => 
    apiClient.get<Department[]>('/departments'),

  getDepartmentById: (id: number) => 
    apiClient.get<Department>(`/departments/${id}`),

  createDepartment: (payload: { name: string }) => 
    apiClient.post<Department>('/departments', payload),

  updateDepartment: (id: number, payload: { name: string; isActive: boolean }) => 
    apiClient.put<Department>(`/departments/${id}`, payload),

  deleteDepartment: (id: number) => 
    apiClient.delete(`/departments/${id}`)
};