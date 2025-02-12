import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type {
  AttendanceRecordDto,
  RecordCardAttendanceCommand,
  RecordOtpAttendanceCommand
} from '@/api/generated/data-contracts';
import { attendanceApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useAttendanceStore = defineStore('attendance', () => {
  // State
  const records = ref<AttendanceRecordDto[]>([]);
  const currentCourseSession = ref<AttendanceRecordDto[] | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const hasUnconfirmedRecords = computed(() =>
    records.value.some(r => !r.isConfirmed)
  );

  // Actions
  async function fetchAttendance(startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.attendanceStudentList({
        startDate: startDate?.toISOString(),
        endDate: endDate?.toISOString()
      });
      records.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function recordCardAttendance(data: RecordCardAttendanceCommand) {
    isLoading.value = true;
    try {
      await attendanceApi.attendanceCardCreate(data);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function recordOtpAttendance(data: RecordOtpAttendanceCommand) {
    isLoading.value = true;
    try {
      await attendanceApi.attendanceOtpCreate(data);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function confirmAttendance(courseSessionId: number) {
    isLoading.value = true;
    try {
      await attendanceApi.attendanceCourseSessionsConfirmCreate(courseSessionId);
      if (currentCourseSession.value) {
        currentCourseSession.value = currentCourseSession.value.map(record => ({
          ...record,
          isConfirmed: true
        }));
      }
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchClassAttendance(courseSessionId: number, date?: Date) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.attendanceCourseSessionsDetail(
        courseSessionId,
        date ? { date: date.toISOString() } : undefined
      );
      currentCourseSession.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchAttendanceStudentList(startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.attendanceStudentList({
        startDate: startDate?.toISOString(),
        endDate: endDate?.toISOString()
      });
      records.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function markAbsent(courseSessionId: number, studentId: number) {
    try {
      await attendanceApi.attendanceCourseSessionsStudentsAbsentCreate(courseSessionId, studentId);
    } catch (err) {
      handleError(err, error);
      throw err;
    }
  }

  return {
    // State
    records,
    currentCourseSession,
    isLoading,
    error,

    // Getters
    hasUnconfirmedRecords,

    // Actions
    fetchAttendance,
    recordCardAttendance,
    recordOtpAttendance,
    confirmAttendance,
    fetchClassAttendance,
    fetchAttendanceStudentList,
    markAbsent
  };
});