import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { Classroom, ReaderDevice } from '@/types/classroom.types';
import { classroomApi } from '@/api/endpoints/classroomApi';

export const useClassroomStore = defineStore('classroom', () => {
  // State
  const classrooms = ref<Classroom[]>([]);
  const currentClassroom = ref<Classroom | null>(null);
  const readerDevices = ref<ReaderDevice[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const availableClassrooms = computed(() => 
    classrooms.value.filter(c => c.isAvailable)
  );

  const classroomsWithDevices = computed(() => 
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

  async function fetchClassroomById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.getById(id);
      currentClassroom.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchReaderDevices() {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.getReaderDevices();
      readerDevices.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function assignReaderDevice(classroomId: number, deviceId: string) {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.assignReader(classroomId, deviceId);
      const index = classrooms.value.findIndex(c => c.id === classroomId);
      if (index !== -1) {
        classrooms.value[index] = data;
      }
      if (currentClassroom.value?.id === classroomId) {
        currentClassroom.value = data;
      }
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function addReaderDevice(device: Partial<ReaderDevice>) {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.addReaderDevice(device);
      readerDevices.value.push(data);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }
  
  async function updateReaderDevice(id: string, status: 'Active' | 'Inactive') {
    isLoading.value = true;
    try {
      const { data } = await classroomApi.updateReaderDevice(id, { status });
      const index = readerDevices.value.findIndex(d => d.id === id);
      if (index !== -1) {
        readerDevices.value[index] = data;
      }
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }
  
  async function removeReaderDevice(classroomId: number) {
    isLoading.value = true;
    try {
      await classroomApi.removeReaderDevice(classroomId);
      const classroom = classrooms.value.find(c => c.id === classroomId);
      if (classroom) {
        classroom.readerDeviceId = undefined;
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
    classrooms,
    currentClassroom,
    readerDevices,
    isLoading,
    error,
    availableClassrooms,
    classroomsWithDevices,
    fetchClassrooms,
    fetchClassroomById,
    fetchReaderDevices,
    assignReaderDevice,
    addReaderDevice,
    updateReaderDevice,
    removeReaderDevice
  };
});