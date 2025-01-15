import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  AcademicYearDto,
  CreateAcademicYearCommand,
  UpdateAcademicYearCommand 
} from '@/api/generated/data-contracts';
import { academicYearApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useAcademicYearStore = defineStore('academicYear', () => {
  // State
  const academicYears = ref<AcademicYearDto[]>([]);
  const currentYear = ref<AcademicYearDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeYear = computed(() => 
    academicYears.value.find(y => y.isActive)
  );

  const sortedYears = computed(() => 
    [...academicYears.value].sort((a, b) => 
      new Date(b.startDate!).getTime() - new Date(a.startDate!).getTime()
    )
  );

  const hasActiveYear = computed(() => 
    academicYears.value.some(y => y.isActive)
  );

  // Actions
  async function fetchAcademicYears() {
    isLoading.value = true;
    try {
      const { data } = await academicYearApi.academicYearList();
      academicYears.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchActiveAcademicYear() {
    isLoading.value = true;
    try {
      const { data } = await academicYearApi.academicYearActiveList();
      currentYear.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createAcademicYear(year: CreateAcademicYearCommand) {
    isLoading.value = true;
    try {
      const { data } = await academicYearApi.academicYearCreate(year);
      academicYears.value.push(data);
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateAcademicYear(id: number, year: UpdateAcademicYearCommand) {
    isLoading.value = true;
    try {
      await academicYearApi.academicYearUpdate(id, year);
      await fetchAcademicYears(); // Refresh list
      if (currentYear.value?.id === id) {
        await fetchActiveAcademicYear(); // Refresh current if updated
      }
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function closeAcademicYear(id: number) {
    isLoading.value = true;
    try {
      await academicYearApi.academicYearCloseCreate(id);
      await fetchAcademicYears(); // Refresh list
      if (currentYear.value?.id === id) {
        currentYear.value = null;
      }
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
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
    fetchActiveAcademicYear,
    createAcademicYear,
    updateAcademicYear,
    closeAcademicYear
  };
});