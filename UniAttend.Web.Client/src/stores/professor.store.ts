import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { ProfessorDto } from '@/api/generated/data-contracts';
import { professorApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useProfessorStore = defineStore('professor', () => {
  // State
  const professors = ref<ProfessorDto[]>([]);
  const currentProfessor = ref<ProfessorDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeProfessors = computed(() => 
    professors.value.filter(p => p.isActive)
  );

    const professorsByDepartment = computed(() => {
    const grouped = new Map<string, ProfessorDto[]>();
    professors.value.forEach(professor => {
      // Get the first department name or default to 'Unassigned'
      const dept = professor.departments?.[0]?.name || 'Unassigned';
      if (!grouped.has(dept)) {
        grouped.set(dept, []);
      }
      grouped.get(dept)?.push(professor);
    });
    return grouped;
  });

  // Actions
  async function fetchProfessors(filters?: { departmentId?: number; isActive?: boolean }) {
    isLoading.value = true;
    try {
      const response = await professorApi.professorList(filters);
      if (response && response.data) {
        professors.value = response.data;
      }
      return response.data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchProfessorById(id: number) {
    isLoading.value = true;
    try {
      const response = await professorApi.professorDetail(id);
      if (response && response.data) {
        currentProfessor.value = response.data;
      }
      return response.data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    // State
    professors,
    currentProfessor,
    isLoading,
    error,
    
    // Getters
    activeProfessors,
    professorsByDepartment,
    
    // Actions
    fetchProfessors,
    fetchProfessorById
  };
});