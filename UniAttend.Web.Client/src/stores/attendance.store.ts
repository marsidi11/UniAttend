import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { attendanceApi } from '@/api/endpoints/attendanceApi';
import type { AttendanceRecord, AttendanceStats, ClassAttendanceResponse } from '@/types/attendance.types';

export const useAttendanceStore = defineStore('attendance', () => {
  const records = ref<AttendanceRecord[]>([]);
  const stats = ref<AttendanceStats | null>(null);
  const isLoading = ref(false);
  const classAttendance = ref<ClassAttendanceResponse | null>(null);

  const attendancePercentage = computed(() =>
    stats.value?.attendanceRate ?? 0);

  async function fetchAttendance(startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.getStudentAttendance(startDate, endDate);
      records.value = data;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStats(groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.getAttendanceStats(groupId);
      stats.value = data;
    } finally {
      isLoading.value = false;
    }
  }

  async function recordAttendance(cardId: string, deviceId: string) {
    return await attendanceApi.recordCardAttendance(cardId, deviceId);
  }

  async function fetchClassAttendance(classId: number): Promise<ClassAttendanceResponse> {
    isLoading.value = true;
    try {
      const { data } = await attendanceApi.getClassAttendance(classId);
      classAttendance.value = {
        records: data.records,
        stats: data.stats
      };
      return classAttendance.value;
    } finally {
      isLoading.value = false;
    }
  }

  async function confirmAttendance(classId: number) {
    return await attendanceApi.confirmClassAttendance(classId);
  }

  return {
    records,
    stats,
    classAttendance,
    isLoading,
    attendancePercentage,
    fetchAttendance,
    fetchStats,
    recordAttendance,
    fetchClassAttendance,
    confirmAttendance
  };
});