import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  AcademicYear, 
  CreateAcademicYearRequest,
  UpdateAcademicYearRequest 
} from '@/types/academicYear.types';
import { academicYearApi } from '@/api/endpoints/academicYearApi';

export const useAcademicYearStore = defineStore('academicYear', () => {
  // State
  const academicYears = ref<AcademicYear[]>([]);
  const currentYear = ref<AcademicYear | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeYear = computed(() => 
    academicYears.value.find(y => y.isActive)
  );

  const sortedYears = computed(() => 
    [...academicYears.value].sort((a, b) => 
      new Date(b.startDate).getTime() - new Date(a.startDate).getTime()
    )
  );

  const hasActiveYear = computed(() => 
    academicYears.value.some(y => y.isActive)
  );

  // Actions
  async function fetchAcademicYears() {
    isLoading.value = true;
    try {
      const { data } = await academicYearApi.getAll();
      academicYears.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchCurrentAcademicYear() {
    isLoading.value = true;
    try {
      const { data } = await academicYearApi.getCurrent();
      currentYear.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function createAcademicYear(year: CreateAcademicYearRequest) {
    isLoading.value = true;
    try {
      const { data } = await academicYearApi.create(year);
      academicYears.value.push(data);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateAcademicYear(id: number, year: UpdateAcademicYearRequest) {
    isLoading.value = true;
    try {
      const { data } = await academicYearApi.update(id, year);
      const index = academicYears.value.findIndex(y => y.id === id);
      if (index !== -1) {
        academicYears.value[index] = data;
      }
      if (currentYear.value?.id === id) {
        currentYear.value = data;
      }
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deleteAcademicYear(id: number) {
    isLoading.value = true;
    try {
      await academicYearApi.delete(id);
      academicYears.value = academicYears.value.filter(y => y.id !== id);
      if (currentYear.value?.id === id) {
        currentYear.value = null;
      }
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function checkDateOverlap(startDate: Date, endDate: Date, excludeId?: number) {
    try {
      const { data } = await academicYearApi.checkOverlap(startDate, endDate, excludeId);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    }
  }

  function handleError(err: unknown) {
    error.value = err instanceof Error ? err.message : 'An error occurred';
  }

  return {
    // State
    academicYears,
    currentYear,
    isLoading,
    error,

    // Getters
    activeYear,
    sortedYears,
    hasActiveYear,

    // Actions
    fetchAcademicYears,
    fetchCurrentAcademicYear,
    createAcademicYear,
    updateAcademicYear,
    deleteAcademicYear,
    checkDateOverlap
  };
});