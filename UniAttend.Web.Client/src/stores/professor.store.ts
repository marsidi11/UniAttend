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

  // Actions
  async function fetchProfessors(filters?: { departmentId?: number; isActive?: boolean }) {
    isLoading.value = true;
    try {
      const { data } = await professorApi.professorList(filters);
      professors.value = data;
      return data;
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
      const { data } = await professorApi.professorDetail(id);
      currentProfessor.value = data;
      return data;
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
    
    // Actions
    fetchProfessors,
    fetchProfessorById
  };
});