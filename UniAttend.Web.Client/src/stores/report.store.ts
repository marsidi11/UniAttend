import { defineStore } from 'pinia';
import { ref } from 'vue';
import type {
  StudentReportDto,
  GroupReportDto,
  DepartmentReportDto,
  AttendanceReportRecordDto,
  AcademicYearReportDto
} from '@/api/generated/data-contracts';
import { reportApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useReportStore = defineStore('report', () => {
  // State
  const studentReport = ref<StudentReportDto | null>(null);
  const groupReport = ref<GroupReportDto | null>(null);
  const departmentReport = ref<DepartmentReportDto | null>(null);
  const attendanceReport = ref<AttendanceReportRecordDto | null>(null);
  const academicYearReport = ref<AcademicYearReportDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Report Retrieval Functions
  async function getStudentReport(id: number, startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const { data } = await reportApi.reportsStudentsDetail(id, {
        startDate: startDate?.toISOString(),
        endDate: endDate?.toISOString()
      });
      studentReport.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getMyReport(startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const { data } = await reportApi.reportsMyReportList({
        startDate: startDate?.toISOString(),
        endDate: endDate?.toISOString()
      });
      studentReport.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getGroupReport(id: number, startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const { data } = await reportApi.reportsGroupsDetail(id, {
        startDate: startDate?.toISOString(),
        endDate: endDate?.toISOString()
      });
      groupReport.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getDepartmentReport(id: number, academicYearId?: number) {
    isLoading.value = true;
    try {
      const { data } = await reportApi.reportsDepartmentsDetail(id, { academicYearId });
      departmentReport.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getAttendanceReport(params: {
    startDate?: Date;
    endDate?: Date;
    departmentId?: number;
    subjectId?: number;
    studyGroupId?: number;
  }) {
    isLoading.value = true;
    try {
      const { data } = await reportApi.reportsAttendanceList({
        startDate: params.startDate?.toISOString(),
        endDate: params.endDate?.toISOString(),
        departmentId: params.departmentId,
        subjectId: params.subjectId,
        studyGroupId: params.studyGroupId
      });
      attendanceReport.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getAcademicYearReport(id: number) {
    isLoading.value = true;
    try {
      const { data } = await reportApi.reportsAcademicYearsDetail(id);
      academicYearReport.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  // Export Functions

  async function exportStudentReport(id: number, startDate?: string, endDate?: string) {
    if (isLoading.value) return;
    isLoading.value = true;
    try {
      const response = await reportApi.reportsExportStudentsDetail(id, {
        startDate,
        endDate
      });
      await downloadFile(response, `student-report-${id}-${new Date().toISOString().split('T')[0]}.pdf`);
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function exportGroupReport(id: number, startDate?: Date, endDate?: Date) {
    if (isLoading.value) return;
    isLoading.value = true;
    try {
      const response = await reportApi.reportsExportGroupsDetail(id, {
        startDate: startDate?.toISOString(),
        endDate: endDate?.toISOString()
      });
      await downloadFile(response, `group-report-${id}-${new Date().toISOString().split('T')[0]}.pdf`);
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function exportDepartmentReport(id: number, academicYearId?: number) {
    if (isLoading.value) return;
    isLoading.value = true;
    try {
      const response = await reportApi.reportsExportDepartmentsDetail(id, { academicYearId });
      await downloadFile(response, `department-report-${id}-${new Date().toISOString().split('T')[0]}.pdf`);
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  // Download file helper
  async function downloadFile(response: Response, filename: string) {
    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    window.URL.revokeObjectURL(url);
  }

  return {
    // State
    studentReport,
    groupReport,
    departmentReport,
    attendanceReport,
    academicYearReport,
    isLoading,
    error,

    // Report Retrieval Actions
    getStudentReport,
    getMyReport,
    getGroupReport,
    getDepartmentReport,
    getAttendanceReport,
    getAcademicYearReport,

    // Export Actions
    exportStudentReport,
    exportGroupReport,
    exportDepartmentReport
  };
});