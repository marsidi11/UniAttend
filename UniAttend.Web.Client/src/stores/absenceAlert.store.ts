import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { AbsenceAlert } from '@/types/alert.types';
import { alertApi } from '@/api/endpoints/alertApi';

export const useAbsenceAlertStore = defineStore('absenceAlert', () => {
  const alerts = ref<AbsenceAlert[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  async function fetchStudentAlerts(studentId: number) {
    isLoading.value = true;
    try {
      const { data } = await alertApi.getStudentAlerts(studentId);
      alerts.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchGroupAlerts(groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await alertApi.getGroupAlerts(groupId);
      alerts.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function markAsSent(alertId: number) {
    isLoading.value = true;
    try {
      await alertApi.markAsSent(alertId);
      const alert = alerts.value.find(a => a.id === alertId);
      if (alert) alert.emailSent = true;
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
    alerts,
    isLoading,
    error,
    fetchStudentAlerts,
    fetchGroupAlerts,
    markAsSent
  };
});