import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { Classroom, CreateClassroomRequest, ReaderDevice } from '@/types/classroom.types';
import { classroomApi } from '@/api/endpoints/classroomApi';

export const useClassroomStore = defineStore('classroom', () => {
  // State
  const classrooms = ref<Classroom[]>([]);
  const currentClassroom = ref<Classroom | null>(null);
  const readers = ref<ReaderDevice[]>([]);
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
      const { data } = await classroomApi.getAll();
      classrooms.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchReaders() {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.getAllReaders();
      readers.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function createClassroom(data: CreateClassroomRequest) {
    isLoading.value = true;
    try {
      const response = await classroomApi.create(data);
      classrooms.value.push(response.data);
      return response.data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateClassroom(id: number, data: Partial<CreateClassroomRequest>) {
    isLoading.value = true;
    try {
      const response = await classroomApi.update(id, data);
      const index = classrooms.value.findIndex(c => c.id === id);
      if (index !== -1) {
        classrooms.value[index] = response.data;
      }
      return response.data;
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
    classrooms,
    currentClassroom,
    readers,
    isLoading,
    error,
    availableClassrooms,
    classroomsWithReaders,
    fetchClassrooms,
    fetchReaders,
    createClassroom,
    updateClassroom
  };
});