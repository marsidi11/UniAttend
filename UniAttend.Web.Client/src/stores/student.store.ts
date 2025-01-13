import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  UserDto as StudentDto,
  RegisterStudentCommand,
  AttendanceRecordDto,
  UserGroupDto
} from '@/api/generated/data-contracts';
import { Student } from '@/api/generated/Student';
import { studentApi } from '@/api/apiInstances';

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

  // Getters updated to use ExtendedStudentDto
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
      const { data } = await studentApi.studentList();
      students.value = data as ExtendedStudentDto[]; // Type assertion since API returns basic UserDto
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function createStudent(studentData: RegisterStudentCommand) {
    isLoading.value = true;
    try {
      const { data: studentId } = await studentApi.studentCreate(studentData);
      await fetchStudentsList(); // Refresh list after creation
      return studentId;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStudentAttendance(startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.studentAttendanceList({
        startDate: startDate?.toISOString(),
        endDate: endDate?.toISOString()
      });
      attendance.value = data as AttendanceRecordDto[]; // Type assertion
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStudentGroups() {
    isLoading.value = true;
    try {
      const { data } = await studentApi.studentEnrolledGroupsList();
      groups.value = data as UserGroupDto[]; // Type assertion
      return data;
    } catch (err) {
      handleError(err);
      throw err;
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
      handleError(err);
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
      handleError(err);
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