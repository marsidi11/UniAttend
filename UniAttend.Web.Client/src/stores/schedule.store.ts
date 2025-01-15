import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  ScheduleDto,
  CreateScheduleCommand,
  UpdateScheduleCommand 
} from '@/api/generated/data-contracts';
import { scheduleApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useScheduleStore = defineStore('schedule', () => {
  // State
  const schedules = ref<ScheduleDto[]>([]);
  const currentSchedule = ref<ScheduleDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const schedulesByDay = computed(() => {
    const grouped = new Map<number, ScheduleDto[]>();
    schedules.value.forEach(schedule => {
      if (schedule.dayOfWeek === undefined) return;
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
      const { data } = classroomId 
        ? await scheduleApi.scheduleClassroomDetail(classroomId)
        : await scheduleApi.scheduleGroupDetail(groupId!);
      schedules.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createSchedule(schedule: CreateScheduleCommand) {
    isLoading.value = true;
    try {
      const { data: scheduleId } = await scheduleApi.scheduleCreate(schedule);
      await fetchSchedules(schedule.groupId);
      return scheduleId;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateSchedule(id: number, schedule: UpdateScheduleCommand) {
    isLoading.value = true;
    try {
      await scheduleApi.scheduleUpdate(id, { id, ...schedule });
      await fetchSchedules(schedule.groupId);
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deleteSchedule(id: number, groupId: number) {
    isLoading.value = true;
    try {
      await scheduleApi.scheduleDelete(id);
      await fetchSchedules(groupId);
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    // State
    schedules,
    currentSchedule,
    isLoading,
    error,

    // Getters
    schedulesByDay,

    // Actions
    fetchSchedules,
    createSchedule,
    updateSchedule,
    deleteSchedule
  };
});