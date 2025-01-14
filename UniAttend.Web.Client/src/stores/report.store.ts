import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { 
  StudentReportDto,
  GroupReportDto,
  DepartmentReportDto,
  AttendanceReportDto,
  AcademicYearReportDto
} from '@/api/generated/data-contracts';
import { reportApi } from '@/api/apiInstances';

export const useReportStore = defineStore('report', () => {
  // State
  const studentReport = ref<StudentReportDto | null>(null);
  const groupReport = ref<GroupReportDto | null>(null);
  const departmentReport = ref<DepartmentReportDto | null>(null);
  const attendanceReport = ref<AttendanceReportDto | null>(null);
  const academicYearReport = ref<AcademicYearReportDto | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Actions
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
      handleError(err);
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
      handleError(err);
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
      handleError(err);
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
    groupId?: number;
  }) {
    isLoading.value = true;
    try {
      const { data } = await reportApi.reportsAttendanceList({
        startDate: params.startDate?.toISOString(),
        endDate: params.endDate?.toISOString(),
        departmentId: params.departmentId,
        subjectId: params.subjectId,
        groupId: params.groupId
      });
      attendanceReport.value = data;
      return data;
    } catch (err) {
      handleError(err);
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
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function exportAttendanceReport(groupId: number, startDate: Date, endDate: Date) {
    isLoading.value = true;
    try {
      await reportApi.reportsExportAttendanceList({
        groupId,
        startDate: startDate.toISOString(),
        endDate: endDate.toISOString()
      });
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
    studentReport,
    groupReport,
    departmentReport,
    attendanceReport,
    academicYearReport,
    isLoading,
    error,

    // Actions
    getStudentReport,
    getGroupReport,
    getDepartmentReport,
    getAttendanceReport,
    getAcademicYearReport,
    exportAttendanceReport
  };
});