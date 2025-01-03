import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { Department } from '@/types/department.types';
import { departmentApi } from '@/api/endpoints/departmentApi';

export const useDepartmentStore = defineStore('department', () => {
  // State
  const departments = ref<Department[]>([]);
  const currentDepartment = ref<Department | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeDepartments = computed(() => 
    departments.value.filter(dept => dept.isActive)
  );

  // Actions
  async function fetchDepartments() {
    isLoading.value = true;
    error.value = null;
    try {
      const { data } = await departmentApi.getDepartments();
      departments.value = data;
      return data;
    } catch (err: any) {
      error.value = err?.response?.data?.message || 'Failed to fetch departments';
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchDepartmentById(id: number) {
    isLoading.value = true;
    error.value = null;
    try {
      const { data } = await departmentApi.getDepartmentById(id);
      currentDepartment.value = data;
      return data;
    } catch (err: any) {
      error.value = err?.response?.data?.message || 'Failed to fetch department';
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createDepartment(name: string) {
    isLoading.value = true;
    error.value = null;
    try {
      const { data } = await departmentApi.createDepartment({ name });
      departments.value.push(data);
      return data;
    } catch (err: any) {
      error.value = err?.response?.data?.message || 'Failed to create department';
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateDepartment(id: number, payload: { name: string; isActive: boolean }) {
    isLoading.value = true;
    error.value = null;
    try {
      const { data } = await departmentApi.updateDepartment(id, payload);
      const index = departments.value.findIndex(d => d.id === id);
      if (index !== -1) {
        departments.value[index] = data;
      }
      if (currentDepartment.value?.id === id) {
        currentDepartment.value = data;
      }
      return data;
    } catch (err: any) {
      error.value = err?.response?.data?.message || 'Failed to update department';
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deleteDepartment(id: number) {
    isLoading.value = true;
    error.value = null;
    try {
      await departmentApi.deleteDepartment(id);
      departments.value = departments.value.filter(d => d.id !== id);
      if (currentDepartment.value?.id === id) {
        currentDepartment.value = null;
      }
    } catch (err: any) {
      error.value = err?.response?.data?.message || 'Failed to delete department';
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    // State
    departments,
    currentDepartment,
    isLoading,
    error,
    
    // Getters
    activeDepartments,
    
    // Actions
    fetchDepartments,
    fetchDepartmentById,
    createDepartment,
    updateDepartment,
    deleteDepartment
  };
});