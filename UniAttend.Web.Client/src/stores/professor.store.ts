import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { Professor, Course, ProfessorSchedule } from '@/types/professor.types';
import { professorApi } from '@/api/endpoints/professorApi';

export const useProfessorStore = defineStore('professor', () => {
  // State
  const professors = ref<Professor[]>([]);
  const currentProfessor = ref<Professor | null>(null);
  const assignedCourses = ref<Course[]>([]);
  const schedule = ref<ProfessorSchedule[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeProfessors = computed(() => 
    professors.value.filter(p => p.isActive)
  );

  const professorsByDepartment = computed(() => {
    const grouped = new Map();
    professors.value.forEach(prof => {
      const dept = prof.departmentName;
      if (!grouped.has(dept)) {
        grouped.set(dept, []);
      }
      grouped.get(dept).push(prof);
    });
    return grouped;
  });

  const currentSchedule = computed(() => 
    schedule.value.filter(s => s.isActive)
  );

  // Actions
  async function fetchProfessors(departmentId?: number) {
    isLoading.value = true;
    try {
      const { data } = await professorApi.getAll(departmentId);
      professors.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchProfessorById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await professorApi.getById(id);
      currentProfessor.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchAssignedCourses(professorId: number) {
    isLoading.value = true;
    try {
      const { data } = await professorApi.getAssignedCourses(professorId);
      assignedCourses.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchSchedule(professorId: number, academicYearId?: number) {
    isLoading.value = true;
    try {
      const { data } = await professorApi.getSchedule(professorId, academicYearId);
      schedule.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function updateProfessor(id: number, data: Partial<Professor>) {
    isLoading.value = true;
    try {
      const response = await professorApi.update(id, data);
      const index = professors.value.findIndex(p => p.id === id);
      if (index !== -1) {
        professors.value[index] = response.data;
      }
      if (currentProfessor.value?.id === id) {
        currentProfessor.value = response.data;
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
    professors,
    currentProfessor,
    assignedCourses,
    schedule,
    isLoading,
    error,
    
    // Getters
    activeProfessors,
    professorsByDepartment,
    currentSchedule,
    
    // Actions
    fetchProfessors,
    fetchProfessorById,
    fetchAssignedCourses,
    fetchSchedule,
    updateProfessor
  };
});