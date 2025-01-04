import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { AcademicYear } from '@/types/academicYear.types';
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

  async function createAcademicYear(year: Partial<AcademicYear>) {
    isLoading.value = true;
    try {
      const { data } = await academicYearApi.create(year);
      academicYears.value.push(data);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateAcademicYear(id: number, year: Partial<AcademicYear>) {
    isLoading.value = true;
    try {
      const { data } = await academicYearApi.update(id, year);
      const index = academicYears.value.findIndex(y => y.id === id);
      if (index !== -1) {
        academicYears.value[index] = data;
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
    academicYears,
    currentYear,
    isLoading,
    error,
    activeYear,
    sortedYears,
    fetchAcademicYears,
    fetchCurrentAcademicYear,
    createAcademicYear,
    updateAcademicYear
  };
});