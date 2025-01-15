import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  DepartmentDto,
  CreateDepartmentCommand,
  UpdateDepartmentCommand 
} from '@/api/generated/data-contracts';
import { departmentApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useDepartmentStore = defineStore('department', () => {
  // State
  const departments = ref<DepartmentDto[]>([]);
  const currentDepartment = ref<DepartmentDto | null>(null);
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
      const { data } = await departmentApi.departmentsList();
      departments.value = data;
    } catch (err) {
      handleError(err, error); // Using handleError with errorRef
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchDepartmentById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await departmentApi.departmentsDetail(id);
      currentDepartment.value = data;
    } catch (err) {
      handleError(err, error); // Using handleError with errorRef
    } finally {
      isLoading.value = false;
    }
  }

  async function createDepartment(request: CreateDepartmentCommand) {
    isLoading.value = true;
    try {
      if (!request.name?.trim()) {
        throw new Error('Department name is required');
      }

      const { data: departmentId } = await departmentApi.departmentsCreate(request);
      const newDepartment = await departmentApi.departmentsDetail(departmentId);
      departments.value.push(newDepartment.data);
      return departmentId;
    } catch (err) {
      handleError(err, error); // Using handleError with errorRef
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateDepartment(id: number, request: UpdateDepartmentCommand) {
    isLoading.value = true;
    try {
      await departmentApi.departmentsUpdate(id, {
        id,
        ...request
      });
      const updatedDepartment = await departmentApi.departmentsDetail(id);
      const index = departments.value.findIndex(d => d.id === id);
      if (index !== -1) {
        departments.value[index] = updatedDepartment.data;
      }
      if (currentDepartment.value?.id === id) {
        currentDepartment.value = updatedDepartment.data;
      }
    } catch (err) {
      handleError(err, error); // Using handleError with errorRef
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function toggleDepartmentStatus(id: number) {
    isLoading.value = true;
    try {
      await departmentApi.departmentsToggleStatusPartialUpdate(id);
      await fetchDepartmentById(id);
      const index = departments.value.findIndex(d => d.id === id);
      if (index !== -1 && currentDepartment.value) {
        departments.value[index] = currentDepartment.value;
      }
    } catch (err) {
      handleError(err, error); // Using handleError with errorRef
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
    toggleDepartmentStatus
  };
});