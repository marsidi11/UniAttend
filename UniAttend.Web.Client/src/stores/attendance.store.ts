import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  AttendanceRecord, 
  AttendanceStats, 
  ClassAttendance,
  RecordCardAttendanceRequest,
  RecordOtpAttendanceRequest 
} from '@/types/attendance.types';
import { attendanceApi } from '@/api/endpoints/attendanceApi';

export const useAttendanceStore = defineStore('attendance', () => {
  // State
  const records = ref<AttendanceRecord[]>([]);
  const stats = ref<AttendanceStats | null>(null);
  const currentClass = ref<ClassAttendance | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const attendanceRate = computed(() => stats.value?.attendanceRate ?? 0);
  const hasUnconfirmedRecords = computed(() => 
    records.value.some(r => !r.isConfirmed)
  );

  // Actions
  async function fetchAttendance(startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.getStudentAttendance(startDate, endDate);
      records.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStats(groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.getAttendanceStats(groupId);
      stats.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function recordCardAttendance(request: RecordCardAttendanceRequest) {
    isLoading.value = true;
    try {
      await attendanceApi.recordCardAttendance(request);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function recordOtpAttendance(request: RecordOtpAttendanceRequest) {
    isLoading.value = true;
    try {
      await attendanceApi.recordOtpAttendance(request);
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function confirmAttendance(classId: number) {
    isLoading.value = true;
    try {
      await attendanceApi.confirmClassAttendance(classId);
      if (currentClass.value) {
        currentClass.value.status = 'Completed';
      }
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchClassAttendance(classId: number) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.getClassAttendance(classId);
      currentClass.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function openClassSession(groupId: number, classroomId: number) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.openClass({ groupId, classroomId });
      currentClass.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }
  
  async function closeClassSession(classId: number) {
    isLoading.value = true;
    try {
      await attendanceApi.closeClass(classId);
      if (currentClass.value?.id === classId) {
        currentClass.value.status = 'Completed';
      }
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function generateAttendanceList(groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.generateList(groupId);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getAbsenceReport(groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.getAbsenceReport(groupId);
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
    records,
    stats,
    currentClass,
    isLoading,
    error,
    
    // Getters
    attendanceRate,
    hasUnconfirmedRecords,
    
    // Actions
    fetchAttendance,
    fetchStats,
    recordCardAttendance,
    recordOtpAttendance,
    confirmAttendance,
    fetchClassAttendance,
    openClassSession,
    closeClassSession,
    generateAttendanceList,
    getAbsenceReport
  };
});