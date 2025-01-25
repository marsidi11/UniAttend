import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  StudyGroupDto,
  GroupStudentDto,
  CreateStudyGroupCommand,
  UpdateStudyGroupCommand,
  TransferStudentCommand 
} from '@/api/generated/data-contracts';
import { studyGroupApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useGroupStore = defineStore('group', () => {
  // State
  const studyGroups = ref<StudyGroupDto[]>([]);
  const currentStudyGroup = ref<StudyGroupDto | null>(null);
  const groupStudents = ref<GroupStudentDto[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeStudyGroups = computed(() => 
    studyGroups.value.filter(g => g.isActive)
  );

  const studyGroupsBySubject = computed(() => {
    const grouped = new Map<string, StudyGroupDto[]>();
    studyGroups.value.forEach(studyGroup => {
      const subject = studyGroup.subjectName || 'Unassigned';
      if (!grouped.has(subject)) {
        grouped.set(subject, []);
      }
      grouped.get(subject)?.push(studyGroup);
    });
    return grouped;
  });

  // Actions
  async function fetchStudyGroups(academicYearId?: number) {
    isLoading.value = true;
    try {
      const { data } = await studyGroupApi.studyGroupsList(
        { academicYearId }
      );
      studyGroups.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStudyGroupById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await studyGroupApi.studyGroupsDetail(id);
      currentStudyGroup.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createStudyGroup(group: CreateStudyGroupCommand) {
    isLoading.value = true;
    try {
      const { data } = await studyGroupApi.studyGroupsCreate(group);
      studyGroups.value.push(data);
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateGroup(id: number, group: UpdateStudyGroupCommand) {
    isLoading.value = true;
    try {
      await studyGroupApi.studyGroupsUpdate(id, { id, ...group });
      const updatedGroup = await fetchStudyGroupById(id);
      const index = studyGroups.value.findIndex(g => g.id === id);
      if (index !== -1) {
        studyGroups.value[index] = updatedGroup;
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
      if (currentStudyGroup.value?.id) {
        await fetchGroupStudents(currentStudyGroup.value.id);
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
    studyGroups,
    currentStudyGroup,
    groupStudents,
    isLoading,
    error,

    // Getters
    activeStudyGroups,
    studyGroupsBySubject,

    // Actions
    fetchStudyGroups,
    fetchStudyGroupById,
    createStudyGroup,
    updateGroup,
    fetchGroupStudents,
    enrollStudents,
    removeStudent,
    transferStudent
  };
});