import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type {
  UserDto as StudentDto,
  RegisterStudentCommand,
  AttendanceRecordDto,
  UserGroupDto
} from '@/api/generated/data-contracts';
import { studentApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

// Extend StudentDto to include cardId
interface ExtendedStudentDto extends StudentDto {
  cardId?: string;
}

export const useStudentStore = defineStore('student', () => {
  // State
  const students = ref<ExtendedStudentDto[]>([]);
  const currentStudent = ref<ExtendedStudentDto | null>(null);
  const attendance = ref<AttendanceRecordDto[]>([]);
  const groups = ref<UserGroupDto[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters remain the same
  const activeStudents = computed(() =>
    students.value.filter((s: ExtendedStudentDto) => s.isActive)
  );

  const groupedByDepartment = computed(() => {
    const grouped = new Map<string, ExtendedStudentDto[]>();
    students.value.forEach((student: ExtendedStudentDto) => {
      const dept = student.departmentName || 'Unassigned';
      if (!grouped.has(dept)) {
        grouped.set(dept, []);
      }
      grouped.get(dept)?.push(student);
    });
    return grouped;
  });

  // Actions
  async function fetchStudentsList() {
    isLoading.value = true;
    try {
      const response = await studentApi.studentList();

      const jsonData = await response.json();

      if (Array.isArray(jsonData)) {
        students.value = jsonData;
        return jsonData;
      }
      // Fallback to checking data property
      else if (jsonData && Array.isArray(jsonData.data)) {
        students.value = jsonData.data;
        return jsonData.data;
      }
      return [];
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createStudent(studentData: RegisterStudentCommand) {
    isLoading.value = true;
    try {
      const response = await studentApi.studentCreate(studentData);
      await fetchStudentsList(); // Refresh list after creation
      return response?.data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStudentAttendance(startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const response = await studentApi.studentAttendanceList({
        startDate: startDate?.toISOString(),
        endDate: endDate?.toISOString()
      });
      // Type assertion for the response
      const attendanceData = response as unknown as { data: AttendanceRecordDto[] };
      if (attendanceData && Array.isArray(attendanceData.data)) {
        attendance.value = attendanceData.data;
        return attendanceData.data;
      }
      return [];
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

    async function fetchStudentGroups() {
    isLoading.value = true;
    try {
      const response = await studentApi.studentEnrolledGroupsList();
      console.log('Raw response:', response);
      
      // If response is already a Response object, parse its JSON
      if (response instanceof Response) {
        const jsonData = await response.json();
        console.log('Parsed JSON:', jsonData);
        groups.value = jsonData;
        return jsonData;
      }
      
      // If response is already parsed JSON
      if (Array.isArray(response)) {
        groups.value = response;
        return response;
      }  
      // Fallback
      groups.value = [];
      return [];
    } catch (err) {
      console.error('Error fetching student groups:', err);
      handleError(err, error);
      groups.value = [];
      return [];
    } finally {
      isLoading.value = false;
    }
  }

  async function getAbsenceAlerts() {
    isLoading.value = true;
    try {
      const { data } = await studentApi.studentAbsenceAlertsList();
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function assignCard(studentId: number, cardId: string) {
    isLoading.value = true;
    try {
      await studentApi.studentCardUpdate(studentId, { studentId, cardId });
      if (currentStudent.value?.id === studentId) {
        currentStudent.value = {
          ...currentStudent.value,
          cardId
        };
      }
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function removeCard(studentId: number) {
    isLoading.value = true;
    try {
      await studentApi.studentCardDelete(studentId);
      if (currentStudent.value?.id === studentId) {
        currentStudent.value = {
          ...currentStudent.value,
          cardId: undefined
        };
      }
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    // State
    students,
    currentStudent,
    attendance,
    groups,
    isLoading,
    error,

    // Getters
    activeStudents,
    groupedByDepartment,

    // Actions
    fetchStudentsList,
    createStudent,
    fetchStudentAttendance,
    fetchStudentGroups,
    getAbsenceAlerts,
    assignCard,
    removeCard
  };
});