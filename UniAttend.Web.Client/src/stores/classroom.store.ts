import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  ClassroomDto,
  CreateClassroomCommand,
  UpdateClassroomCommand,
  AssignReaderCommand 
} from '@/api/generated/data-contracts';
import { classroomApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useClassroomStore = defineStore('classroom', () => {
  // State
  const classrooms = ref<ClassroomDto[]>([]);
  const currentCourseSessionroom = ref<ClassroomDto | null>(null);
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
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getClassroomById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.classroomsDetail(id);
      currentCourseSessionroom.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
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
      handleError(err, error);
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
      if (currentCourseSessionroom.value?.id === id) {
        currentCourseSessionroom.value = updatedClassroom;
      }
      return updatedClassroom;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

    async function assignReader(id: number, deviceId: string) {
      isLoading.value = true;
      try {
        await classroomApi.classroomsReaderCreate(id, { 
          classroomId: id,
          readerDeviceId: deviceId 
        });
        await getClassroomById(id);
      } catch (err) {
        handleError(err, error);
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
      handleError(err, error);
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
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    // State
    classrooms,
    currentCourseSessionroom,
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