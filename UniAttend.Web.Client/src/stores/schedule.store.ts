import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type {
  ScheduleDto,
  CreateScheduleCommand,
  UpdateScheduleCommand
} from '@/api/generated/data-contracts';
import { scheduleApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';
import { useToast } from 'vue-toastification';

export const useScheduleStore = defineStore('schedule', () => {
  // State
  const schedules = ref<ScheduleDto[]>([]);
  const currentSchedule = ref<ScheduleDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);
  const toast = useToast();

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
  async function fetchAllSchedules() {
    isLoading.value = true
    try {
      const { data } = await scheduleApi.scheduleList()
      schedules.value = data
      return data
    } catch (err) {
      handleError(err, error)
      throw err
    } finally {
      isLoading.value = false
    }
  }

  async function fetchSchedules(
    studyGroupId?: number, 
    classroomId?: number, 
    professorId?: number
  ) {
    isLoading.value = true;
    try {
      let data;
      if (professorId) {
        ({ data } = await scheduleApi.scheduleProfessorDetail(professorId));
      } else if (classroomId) {
        ({ data } = await scheduleApi.scheduleClassroomDetail(classroomId));
      } else if (studyGroupId) {
        ({ data } = await scheduleApi.scheduleGroupDetail(studyGroupId));
      } else {
        ({ data } = await scheduleApi.scheduleList());
      }
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
      await fetchSchedules(schedule.studyGroupId);
      toast.success('Schedule created successfully');
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
      await fetchSchedules(schedule.studyGroupId);
      toast.success('Schedule updated successfully');
    } catch (err: any) {
      if (err.response?.data?.message) {
        toast.error(err.response.data.message);
      } else {
        toast.error('Failed to update schedule');
      }
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deleteSchedule(id: number, studyGroupId: number) {
    isLoading.value = true;
    try {
      await scheduleApi.scheduleDelete(id);
      await fetchSchedules(studyGroupId);
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
    fetchAllSchedules,
    fetchSchedules,
    createSchedule,
    updateSchedule,
    deleteSchedule
  };
});