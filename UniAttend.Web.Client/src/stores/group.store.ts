import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { StudyGroup, GroupStudent } from '@/types/group.types';
import { groupApi } from '@/api/endpoints/groupApi';

export const useGroupStore = defineStore('group', () => {
  // State
  const groups = ref<StudyGroup[]>([]);
  const currentGroup = ref<StudyGroup | null>(null);
  const groupStudents = ref<GroupStudent[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeGroups = computed(() => 
    groups.value.filter(g => g.isActive)
  );

  const groupsBySubject = computed(() => {
    const grouped = new Map();
    groups.value.forEach(group => {
      const subject = group.subjectName;
      if (!grouped.has(subject)) {
        grouped.set(subject, []);
      }
      grouped.get(subject).push(group);
    });
    return grouped;
  });

  // Actions
  async function fetchGroups(academicYearId?: number) {
    isLoading.value = true;
    try {
      const { data } = await groupApi.getAll(academicYearId);
      groups.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchGroupById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await groupApi.getById(id);
      currentGroup.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchGroupStudents(groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await groupApi.getStudents(groupId);
      groupStudents.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function addStudentToGroup(groupId: number, studentId: number) {
    isLoading.value = true;
    try {
      await groupApi.addStudent(groupId, studentId);
      await fetchGroupStudents(groupId);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function removeStudentFromGroup(groupId: number, studentId: number) {
    isLoading.value = true;
    try {
      await groupApi.removeStudent(groupId, studentId);
      groupStudents.value = groupStudents.value.filter(
        gs => gs.studentId !== studentId
      );
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function enrollStudents(groupId: number, studentIds: number[]) {
    isLoading.value = true;
    try {
      await groupApi.enrollStudents(groupId, studentIds);
      await fetchGroupStudents(groupId);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }
  
  async function unenrollStudent(groupId: number, studentId: number) {
    isLoading.value = true;
    try {
      await groupApi.unenrollStudent(groupId, studentId);
      groupStudents.value = groupStudents.value
        .filter(gs => gs.studentId !== studentId);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }
  
  async function transferStudent(
    studentId: number, 
    fromGroupId: number, 
    toGroupId: number
  ) {
    isLoading.value = true;
    try {
      await groupApi.transferStudent(studentId, fromGroupId, toGroupId);
      await Promise.all([
        fetchGroupStudents(fromGroupId),
        fetchGroupStudents(toGroupId)
      ]);
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
    groups,
    currentGroup,
    groupStudents,
    isLoading,
    error,

    // Getters
    activeGroups,
    groupsBySubject,

    // Actions
    fetchGroups,
    fetchGroupById,
    fetchGroupStudents,
    addStudentToGroup,
    removeStudentFromGroup,
    enrollStudents,
    unenrollStudent,
    transferStudent
  };
});