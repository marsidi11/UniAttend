import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { Schedule } from '@/types/schedule.types';
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

  // Actions
  async function fetchSchedules(groupId?: number, classroomId?: number) {
    isLoading.value = true;
    try {
      const { data } = await scheduleApi.getAll({ groupId, classroomId });
      schedules.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function createSchedule(schedule: Partial<Schedule>) {
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

  async function updateSchedule(id: number, schedule: Partial<Schedule>) {
    isLoading.value = true;
    try {
      const { data } = await scheduleApi.update(id, schedule);
      const index = schedules.value.findIndex(s => s.id === id);
      if (index !== -1) {
        schedules.value[index] = data;
      }
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
    schedules,
    currentSchedule,
    isLoading,
    error,
    schedulesByDay,
    fetchSchedules,
    createSchedule,
    updateSchedule
  };
});