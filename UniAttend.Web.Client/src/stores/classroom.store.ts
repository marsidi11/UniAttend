import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  ClassroomDto,
  CreateClassroomCommand,
  UpdateClassroomCommand,
  AssignReaderCommand 
} from '@/api/generated/data-contracts';
import { classroomApi } from '@/api/apiInstances';

export const useClassroomStore = defineStore('classroom', () => {
  // State
  const classrooms = ref<ClassroomDto[]>([]);
  const currentClassroom = ref<ClassroomDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const availableClassrooms = computed(() => 
    classrooms.value.filter(c => c.status === 'available')
  );

  const classroomsWithReaders = computed(() =>
    classrooms.value.filter(c => c.readerDeviceId)
  );

  // Actions
  async function fetchClassrooms() {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.classroomsList();
      classrooms.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getClassroomById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.classroomsDetail(id);
      currentClassroom.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createClassroom(classroom: CreateClassroomCommand) {
    isLoading.value = true;
    try {
      const { data: id } = await classroomApi.classroomsCreate(classroom);
      const newClassroom = await getClassroomById(id);
      classrooms.value.push(newClassroom);
      return newClassroom;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateClassroom(id: number, classroom: UpdateClassroomCommand) {
    isLoading.value = true;
    try {
      await classroomApi.classroomsUpdate(id, { id, ...classroom });
      const updatedClassroom = await getClassroomById(id);
      const index = classrooms.value.findIndex(c => c.id === id);
      if (index !== -1) {
        classrooms.value[index] = updatedClassroom;
      }
      if (currentClassroom.value?.id === id) {
        currentClassroom.value = updatedClassroom;
      }
      return updatedClassroom;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function assignReader(id: number, deviceId: string) {
    isLoading.value = true;
    try {
      await classroomApi.classroomsReaderCreate(id, { deviceId });
      await getClassroomById(id);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function removeReader(id: number) {
    isLoading.value = true;
    try {
      await classroomApi.classroomsReaderDelete(id);
      await getClassroomById(id);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getAvailableClassrooms(startTime: Date, endTime: Date) {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.classroomsAvailableList({
        startTime: startTime.toISOString(),
        endTime: endTime.toISOString()
      });
      return data;
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
    classrooms,
    currentClassroom,
    isLoading,
    error,
    
    // Getters
    availableClassrooms,
    classroomsWithReaders,
    
    // Actions
    fetchClassrooms,
    getClassroomById,
    createClassroom,
    updateClassroom,
    assignReader,
    removeReader,
    getAvailableClassrooms
  };
});