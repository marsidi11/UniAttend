import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  ClassDto,
  OpenClassCommand
} from '@/api/generated/data-contracts';
import { classApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useClassStore = defineStore('class', () => {
  // State
  const classes = ref<ClassDto[]>([]);
  const currentClass = ref<ClassDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeClasses = computed(() => 
    classes.value.filter(c => c.status === 'active')
  );

  const classesByGroup = computed(() => {
    const grouped = new Map<string, ClassDto[]>();
    classes.value.forEach(cls => {
      const groupName = cls.groupName || 'Unassigned';
      if (!grouped.has(groupName)) {
        grouped.set(groupName, []);
      }
      grouped.get(groupName)?.push(cls);
    });
    return grouped;
  });

  // Actions
  async function fetchClasses(filters?: { 
    groupId?: number;
    classroomId?: number;
    date?: Date;
  }) {
    isLoading.value = true;
    try {
      const { data } = await classApi.classesList({
        ...filters,
        date: filters?.date?.toISOString()
      });
      classes.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getClassById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await classApi.classesDetail(id);
      currentClass.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function openClass(classData: OpenClassCommand) {
    isLoading.value = true;
    try {
      const { data } = await classApi.classesCreate(classData);
      classes.value.push(data);
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function closeClass(id: number) {
    isLoading.value = true;
    try {
      await classApi.classesCloseCreate(id);
      const index = classes.value.findIndex(c => c.id === id);
      if (index !== -1) {
        classes.value[index] = { ...classes.value[index], status: 'closed' };
      }
      if (currentClass.value?.id === id) {
        currentClass.value = { ...currentClass.value, status: 'closed' };
      }
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getClassesByGroup(groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await classApi.classesGroupDetail(groupId);
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getClassesByClassroom(classroomId: number) {
    isLoading.value = true;
    try {
      const { data } = await classApi.classesClassroomDetail(classroomId);
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
    classes,
    currentClass,
    isLoading,
    error,
    
    // Getters
    activeClasses,
    classesByGroup,
    
    // Actions
    fetchClasses,
    getClassById,
    openClass,
    closeClass,
    getClassesByGroup,
    getClassesByClassroom
  };
});