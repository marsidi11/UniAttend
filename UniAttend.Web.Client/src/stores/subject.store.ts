import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { SubjectDto, CreateSubjectCommand, UpdateSubjectCommand } from '@/api/generated/data-contracts';
import { subjectApi } from '@/api/apiInstances';

export const useSubjectStore = defineStore('subject', () => {
  // State
  const subjects = ref<SubjectDto[]>([]);
  const currentSubject = ref<SubjectDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeSubjects = computed(() => 
    subjects.value.filter(s => s.isActive)
  );

  const subjectsByDepartment = computed(() => {
    const grouped = new Map<string, SubjectDto[]>();
    subjects.value.forEach(subject => {
      const dept = subject.departmentName;
      if (!grouped.has(dept)) {
        grouped.set(dept, []);
      }
      grouped.get(dept)?.push(subject);
    });
    return grouped;
  });

  // Actions
  async function fetchSubjects(filters?: { departmentId?: number; isActive?: boolean }) {
    isLoading.value = true;
    try {
      const { data } = await subjectApi.subjectsList(filters);
      subjects.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchSubjectById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await subjectApi.subjectsDetail(id);
      currentSubject.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createSubject(subject: CreateSubjectCommand) {
    isLoading.value = true;
    try {
      const { data } = await subjectApi.subjectsCreate(subject);
      subjects.value.push(data);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateSubject(id: number, subject: UpdateSubjectCommand) {
    isLoading.value = true;
    try {
      await subjectApi.subjectsUpdate(id, { id, ...subject });
      const updatedSubject = await subjectApi.subjectsDetail(id);
      const index = subjects.value.findIndex(s => s.id === id);
      if (index !== -1) {
        subjects.value[index] = updatedSubject.data;
      }
      if (currentSubject.value?.id === id) {
        currentSubject.value = updatedSubject.data;
      }
      return updatedSubject.data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deleteSubject(id: number) {
    isLoading.value = true;
    try {
      await subjectApi.subjectsDelete(id);
      subjects.value = subjects.value.filter(s => s.id !== id);
      if (currentSubject.value?.id === id) {
        currentSubject.value = null;
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
    updateSubject,
    deleteSubject
  };
});