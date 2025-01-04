import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { Subject } from '@/types/subject.types';
import { subjectApi } from '@/api/endpoints/subjectApi';

export const useSubjectStore = defineStore('subject', () => {
  // State
  const subjects = ref<Subject[]>([]);
  const currentSubject = ref<Subject | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeSubjects = computed(() => 
    subjects.value.filter(s => s.isActive)
  );

  const subjectsByDepartment = computed(() => {
    const grouped = new Map();
    subjects.value.forEach(subject => {
      const dept = subject.departmentName;
      if (!grouped.has(dept)) {
        grouped.set(dept, []);
      }
      grouped.get(dept).push(subject);
    });
    return grouped;
  });

  // Actions
  async function fetchSubjects(departmentId?: number) {
    isLoading.value = true;
    try {
      const { data } = await subjectApi.getAll(departmentId);
      subjects.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchSubjectById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await subjectApi.getById(id);
      currentSubject.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function createSubject(subject: Partial<Subject>) {
    isLoading.value = true;
    try {
      const { data } = await subjectApi.create(subject);
      subjects.value.push(data);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateSubject(id: number, subject: Partial<Subject>) {
    isLoading.value = true;
    try {
      const { data } = await subjectApi.update(id, subject);
      const index = subjects.value.findIndex(s => s.id === id);
      if (index !== -1) {
        subjects.value[index] = data;
      }
      if (currentSubject.value?.id === id) {
        currentSubject.value = data;
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
    subjects,
    currentSubject,
    isLoading,
    error,
    
    // Getters
    activeSubjects,
    subjectsByDepartment,
    
    // Actions
    fetchSubjects,
    fetchSubjectById,
    createSubject,
    updateSubject
  };
});