import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  Schedule, 
  CreateScheduleRequest, 
  UpdateScheduleRequest 
} from '@/types/schedule.types';
import { scheduleApi } from '@/api/endpoints/scheduleApi';

export const useScheduleStore = defineStore('schedule', () => {
  // State
  const schedules = ref<Schedule[]>([]);
  const currentSchedule = ref<Schedule | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const schedulesByDay = computed(() => {
    const grouped = new Map<number, Schedule[]>();
    schedules.value.forEach(schedule => {
      if (!grouped.has(schedule.dayOfWeek)) {
        grouped.set(schedule.dayOfWeek, []);
      }
      grouped.get(schedule.dayOfWeek)?.push(schedule);
    });
    return grouped;
  });

  const groupSchedules = computed(() => 
    (groupId: number) => schedules.value.filter(s => s.groupId === groupId)
  );

  const classroomSchedules = computed(() => 
    (classroomId: number) => schedules.value.filter(s => s.classroomId === classroomId)
  );

  // Actions
  async function fetchSchedules(params?: { groupId?: number; classroomId?: number }) {
    isLoading.value = true;
    try {
      const { data } = await scheduleApi.getAll(params);
      schedules.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchScheduleById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await scheduleApi.getById(id);
      currentSchedule.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createSchedule(schedule: CreateScheduleRequest) {
    isLoading.value = true;
    try {
      const { data } = await scheduleApi.create(schedule);
      schedules.value.push(data);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateSchedule(id: number, schedule: UpdateScheduleRequest) {
    isLoading.value = true;
    try {
      const { data } = await scheduleApi.update(id, schedule);
      const index = schedules.value.findIndex(s => s.id === id);
      if (index !== -1) {
        schedules.value[index] = data;
      }
      if (currentSchedule.value?.id === id) {
        currentSchedule.value = data;
      }
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deleteSchedule(id: number) {
    isLoading.value = true;
    try {
      await scheduleApi.delete(id);
      schedules.value = schedules.value.filter(s => s.id !== id);
      if (currentSchedule.value?.id === id) {
        currentSchedule.value = null;
      }
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function checkScheduleConflict(request: ScheduleConflictRequest) {
    try {
      const { data } = await scheduleApi.checkConflict(request);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    }
  }

  function handleError(err: unknown) {
    error.value = err instanceof Error ? err.message : 'An error occurred';
  }

  return {
    // State
    schedules,
    currentSchedule,
    isLoading,
    error,

    // Getters
    schedulesByDay,
    groupSchedules,
    classroomSchedules,

    // Actions
    fetchSchedules,
    fetchScheduleById,
    createSchedule,
    updateSchedule,
    deleteSchedule,
    checkScheduleConflict
  };
});