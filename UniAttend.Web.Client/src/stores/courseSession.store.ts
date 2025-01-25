import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  CourseSessionDto,
  OpenCourseSessionCommand
} from '@/api/generated/data-contracts';
import { courseSessionApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useCourseSessiontore = defineStore('class', () => {
  // State
  const courseSessions = ref<CourseSessionDto[]>([]);
  const currentCourseSession = ref<CourseSessionDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeCourseSessions = computed(() => 
    courseSessions.value.filter(c => c.status === 'active')
  );

  const courseSessionsByGroup = computed(() => {
    const grouped = new Map<string, CourseSessionDto[]>();
    courseSessions.value.forEach(cls => {
      const studyGroupName = cls.studyGroupName || 'Unassigned';
      if (!grouped.has(studyGroupName)) {
        grouped.set(studyGroupName, []);
      }
      grouped.get(studyGroupName)?.push(cls);
    });
    return grouped;
  });

  // Actions
  async function fetchCourseSessions(filters?: { 
    studyGroupId?: number;
    classroomId?: number;
    date?: Date;
  }) {
    isLoading.value = true;
    try {
      const { data } = await courseSessionApi.courseSessionsList({
        ...filters,
        date: filters?.date?.toISOString()
      });
      courseSessions.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getCourseSessionById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await courseSessionApi.courseSessionsDetail(id);
      currentCourseSession.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function OpenCourseSession(courseSession: OpenCourseSessionCommand) {
    isLoading.value = true;
    try {
      const { data } = await courseSessionApi.courseSessionsCreate(courseSession);
      courseSessions.value.push(data);
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function closeCourseSession(id: number) {
    isLoading.value = true;
    try {
      await courseSessionApi.courseSessionsCloseCreate(id);
      const index = courseSessions.value.findIndex(c => c.id === id);
      if (index !== -1) {
        courseSessions.value[index] = { ...courseSessions.value[index], status: 'closed' };
      }
      if (currentCourseSession.value?.id === id) {
        currentCourseSession.value = { ...currentCourseSession.value, status: 'closed' };
      }
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getCourseSessionsByGroup(studyGroupId: number) {
    isLoading.value = true;
    try {
      const { data } = await courseSessionApi.courseSessionsGroupDetail(studyGroupId);
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getCourseSessionsByClassroom(classroomId: number) {
    isLoading.value = true;
    try {
      const { data } = await courseSessionApi.courseSessionsClassroomDetail(classroomId);
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
    courseSessions,
    currentCourseSession,
    isLoading,
    error,
    
    // Getters
    activeCourseSessions,
    courseSessionsByGroup,
    
    // Actions
    fetchCourseSessions,
    getCourseSessionById,
    OpenCourseSession,
    closeCourseSession,
    getCourseSessionsByGroup,
    getCourseSessionsByClassroom
  };
});