import apiClient from '../apiClient';
import type { AbsenceAlert, AlertSummary } from '@/types/alert.types';

export const alertApi = {
  // Student alerts
  getStudentAlerts: (studentId: number) =>
    apiClient.get<AbsenceAlert[]>(`/alerts/student/${studentId}`),

  // Group alerts
  getGroupAlerts: (groupId: number) =>
    apiClient.get<AbsenceAlert[]>(`/alerts/group/${groupId}`),

  // Get alert summary
  getAlertSummary: () =>
    apiClient.get<AlertSummary>('/alerts/summary'),

  // Get unset alerts
  getUnsentAlerts: () =>
    apiClient.get<AbsenceAlert[]>('/alerts/unsent'),

  // Mark alert as sent
  markAsSent: (alertId: number) =>
    apiClient.put(`/alerts/${alertId}/mark-sent`),

  // Create new alert
  createAlert: (data: Omit<AbsenceAlert, 'id' | 'createdAt'>) =>
    apiClient.post<AbsenceAlert>('/alerts', data),

  // Delete alert
  deleteAlert: (alertId: number) =>
    apiClient.delete(`/alerts/${alertId}`),

  // Get alerts by date range
  getAlertsByDateRange: (startDate: Date, endDate: Date) =>
    apiClient.get<AbsenceAlert[]>('/alerts', {
      params: { startDate, endDate }
    }),

  // Get critical alerts (> 25% absence)
  getCriticalAlerts: () =>
    apiClient.get<AbsenceAlert[]>('/alerts/critical')
};