import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  AbsenceAlert, 
  AlertSummary, 
  CreateAlertRequest 
} from '@/types/alert.types';
import { alertApi } from '@/api/endpoints/alertApi';

export const useAbsenceAlertStore = defineStore('absenceAlert', () => {
  // State
  const alerts = ref<AbsenceAlert[]>([]);
  const summary = ref<AlertSummary | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const unsentAlerts = computed(() => 
    alerts.value.filter(alert => !alert.emailSent)
  );

  const criticalAlerts = computed(() => 
    alerts.value.filter(alert => alert.absencePercentage > 25)
  );

  const alertsByStudent = computed(() => {
    const map = new Map<number, AbsenceAlert[]>();
    alerts.value.forEach(alert => {
      const alerts = map.get(alert.studentId) || [];
      alerts.push(alert);
      map.set(alert.studentId, alerts);
    });
    return map;
  });

  // Actions
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

  async function fetchAlertSummary() {
    isLoading.value = true;
    try {
      const { data } = await alertApi.getAlertSummary();
      summary.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createAlert(data: CreateAlertRequest) {
    isLoading.value = true;
    try {
      const response = await alertApi.createAlert(data);
      alerts.value.push(response.data);
      return response.data;
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
      await alertApi.markAsSent({ alertId, notificationSent: true });
      const alert = alerts.value.find(a => a.id === alertId);
      if (alert) {
        alert.emailSent = true;
      }
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deleteAlert(alertId: number) {
    isLoading.value = true;
    try {
      await alertApi.deleteAlert(alertId);
      alerts.value = alerts.value.filter(a => a.id !== alertId);
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
    alerts,
    summary,
    isLoading,
    error,

    // Getters
    unsentAlerts,
    criticalAlerts,
    alertsByStudent,

    // Actions
    fetchStudentAlerts,
    fetchGroupAlerts,
    fetchAlertSummary,
    createAlert,
    markAsSent,
    deleteAlert
  };
});