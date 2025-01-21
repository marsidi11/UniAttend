import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  StudyGroupDto,
  GroupStudentDto,
  CreateGroupCommand,
  UpdateGroupCommand,
  TransferStudentCommand 
} from '@/api/generated/data-contracts';
import { studyGroupApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useGroupStore = defineStore('group', () => {
  // State
  const groups = ref<StudyGroupDto[]>([]);
  const currentGroup = ref<StudyGroupDto | null>(null);
  const groupStudents = ref<GroupStudentDto[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeGroups = computed(() => 
    groups.value.filter(g => g.isActive)
  );

  const groupsBySubject = computed(() => {
    const grouped = new Map<string, StudyGroupDto[]>();
    groups.value.forEach(group => {
      const subject = group.subjectName || 'Unassigned';
      if (!grouped.has(subject)) {
        grouped.set(subject, []);
      }
      grouped.get(subject)?.push(group);
    });
    return grouped;
  });

  // Actions
  async function fetchGroups(academicYearId?: number) {
    isLoading.value = true;
    try {
      const { data } = await studyGroupApi.studyGroupsList(
        { academicYearId }
      );
      groups.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchGroupById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await studyGroupApi.studyGroupsDetail(id);
      currentGroup.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createGroup(group: CreateGroupCommand) {
    isLoading.value = true;
    try {
      const { data } = await studyGroupApi.studyGroupsCreate(group);
      groups.value.push(data);
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateGroup(id: number, group: UpdateGroupCommand) {
    isLoading.value = true;
    try {
      await studyGroupApi.studyGroupsUpdate(id, { id, ...group });
      const updatedGroup = await fetchGroupById(id);
      const index = groups.value.findIndex(g => g.id === id);
      if (index !== -1) {
        groups.value[index] = updatedGroup;
      }
      return updatedGroup;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchGroupStudents(id: number) {
    isLoading.value = true;
    try {
      const { data } = await studyGroupApi.studyGroupsStudentsDetail(id);
      groupStudents.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function enrollStudents(id: number, studentIds: number[]) {
    isLoading.value = true;
    try {
      await studyGroupApi.studyGroupsStudentsEnrollCreate(id, { studyGroupId: id, studentIds });
      await fetchGroupStudents(id);
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function removeStudent(studyGroupId: number, studentId: number) {
    isLoading.value = true;
    try {
      await studyGroupApi.studyGroupsStudentsDelete(studyGroupId, studentId);
      groupStudents.value = groupStudents.value.filter(
        gs => gs.studentId !== studentId
      );
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function transferStudent(data: TransferStudentCommand) {
    isLoading.value = true;
    try {
      await studyGroupApi.studyGroupsTransferStudentCreate(data);
      if (currentGroup.value?.id) {
        await fetchGroupStudents(currentGroup.value.id);
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
    createGroup,
    updateGroup,
    fetchGroupStudents,
    enrollStudents,
    removeStudent,
    transferStudent
  };
});