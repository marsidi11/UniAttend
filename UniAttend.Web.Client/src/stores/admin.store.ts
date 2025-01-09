import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  CreateStaffCommand, 
  User, 
  Department,
  AcademicYear,
  Subject,
  SystemStats,
  AdminAttendanceReport,
  SystemLog,
  BackupInfo
} from '@/types/admin.types';
import { adminApi } from '@/api/endpoints/adminApi';

export const useAdminStore = defineStore('admin', () => {
  // State
  const staff = ref<User[]>([]);
  const departments = ref<Department[]>([]);
  const academicYears = ref<AcademicYear[]>([]);
  const subjects = ref<Subject[]>([]);
  const systemStats = ref<SystemStats | null>(null);
  const systemLogs = ref<SystemLog[]>([]);
  const backups = ref<BackupInfo[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeStaff = computed(() => 
    staff.value.filter(s => s.isActive)
  );

  const activeDepartments = computed(() => 
    departments.value.filter(d => d.isActive)
  );

  const currentAcademicYear = computed(() => 
    academicYears.value.find(ay => ay.isActive)
  );

  // Staff Management Actions
  async function createStaff(command: CreateStaffCommand) {
    isLoading.value = true;
    try {
      const { data } = await adminApi.createStaff(command);
      staff.value.push(data);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateStaff(id: number, data: UpdateStaffRequest) {
    isLoading.value = true;
    try {
        const response = await adminApi.updateStaff(id, data);
        const index = staff.value.findIndex(s => s.id === id);
        if (index !== -1) {
            staff.value[index] = { ...staff.value[index], ...response.data };
        }
        return response.data;
    } catch (err) {
        handleError(err);
        throw err;
    } finally {
        isLoading.value = false;
    }
}

  async function getStaffList() {
    isLoading.value = true;
    try {
      const { data } = await adminApi.getStaffList();
      staff.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  // Department Management Actions
  async function createDepartment(name: string) {
    isLoading.value = true;
    try {
      const { data } = await adminApi.createDepartment({ name });
      departments.value.push(data);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateDepartment(id: number, data: { name: string; isActive: boolean }) {
    isLoading.value = true;
    try {
      const response = await adminApi.updateDepartment(id, data);
      const index = departments.value.findIndex(d => d.id === id);
      if (index !== -1) {
        departments.value[index] = { ...departments.value[index], ...data };
      }
      return response;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  // Academic Year Management Actions
  async function createAcademicYear(data: { 
    name: string;
    startDate: Date;
    endDate: Date;
  }) {
    isLoading.value = true;
    try {
      const response = await adminApi.createAcademicYear(data);
      academicYears.value.push(response.data);
      return response.data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  // Subject Management Actions
  async function createSubject(data: {
    name: string;
    departmentId: number;
  }) {
    isLoading.value = true;
    try {
      const response = await adminApi.createSubject(data);
      subjects.value.push(response.data);
      return response.data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  // System Management Actions
  async function getSystemStats() {
    isLoading.value = true;
    try {
      const { data } = await adminApi.getSystemStats();
      systemStats.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getAttendanceReport(params: {
    departmentId?: number;
    startDate?: Date;
    endDate?: Date;
  }) {
    isLoading.value = true;
    try {
      const { data } = await adminApi.getAttendanceReport(params);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getSystemLogs(params?: {
    startDate?: Date;
    endDate?: Date;
    level?: 'Info' | 'Warning' | 'Error';
  }) {
    isLoading.value = true;
    try {
      const { data } = await adminApi.getSystemLogs(params);
      systemLogs.value = data;
      return data;
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
    staff,
    departments,
    academicYears,
    subjects,
    systemStats,
    systemLogs,
    backups,
    isLoading,
    error,

    // Getters
    activeStaff,
    activeDepartments,
    currentAcademicYear,

    // Actions
    createStaff,
    updateStaff,
    getStaffList,
    createDepartment,
    updateDepartment,
    createAcademicYear,
    createSubject,
    getSystemStats,
    getAttendanceReport,
    getSystemLogs
  };
});