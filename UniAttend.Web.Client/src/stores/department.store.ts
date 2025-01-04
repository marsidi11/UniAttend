import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  Department, 
  CreateDepartmentRequest, 
  UpdateDepartmentRequest 
} from '@/types/department.types';
import { departmentApi } from '@/api/endpoints/departmentApi';

export const useDepartmentStore = defineStore('department', () => {
  // State
  const departments = ref<Department[]>([]);
  const currentDepartment = ref<Department | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeDepartments = computed(() => 
    departments.value.filter(d => d.isActive)
  );

  // Actions
  async function fetchDepartments() {
    isLoading.value = true;
    try {
      const { data } = await departmentApi.getAll();
      departments.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchDepartmentById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await departmentApi.getById(id);
      currentDepartment.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function createDepartment(request: CreateDepartmentRequest) {
    isLoading.value = true;
    try {
      const { data } = await departmentApi.create(request);
      departments.value.push(data);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateDepartment(id: number, request: UpdateDepartmentRequest) {
    isLoading.value = true;
    try {
      const { data } = await departmentApi.update(id, request);
      const index = departments.value.findIndex(d => d.id === id);
      if (index !== -1) {
        departments.value[index] = data;
      }
      if (currentDepartment.value?.id === id) {
        currentDepartment.value = data;
      }
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  function handleError(err: unknown) {
    error.value = err instanceof Error ? err.message : 'An error occurred';
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
    updateDepartment
  };
});