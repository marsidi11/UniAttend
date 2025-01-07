import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  Student,
  StudentProfile, 
  StudentGroupDetails,
  StudentStats,
  AttendanceRecord,
  UpdateStudentRequest 
} from '@/types/student.types';
import { studentApi } from '@/api/endpoints/studentApi';

export const useStudentStore = defineStore('student', () => {
  // State
  const students = ref<Student[]>([]);
  const currentStudent = ref<Student | null>(null);
  const attendance = ref<AttendanceRecord[]>([]);
  const groups = ref<StudentGroupDetails[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeStudents = computed(() => 
    students.value.filter(s => s.isActive)
  );

  const groupedByDepartment = computed(() => {
    const grouped = new Map<string, Student[]>();
    students.value.forEach(student => {
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
        const { data } = await studentApi.getAll();
        students.value = data;
        return data;
    } catch (err) {
        handleError(err);
        throw err;
    } finally {
        isLoading.value = false;
    }
}

  async function createStudent(studentData: UpdateStudentRequest) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getProfile();
      students.value.push(data);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateStudent(id: number, studentData: UpdateStudentRequest) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.updateProfile(studentData);
      const index = students.value.findIndex(s => s.id === id);
      if (index !== -1) {
        students.value[index] = { ...students.value[index], ...data };
      }
      if (currentStudent.value?.id === id) {
        currentStudent.value = { ...currentStudent.value, ...data };
      }
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStudentById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getProfile();
      currentStudent.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStudentAttendance(studentId: number, startDate?: Date, endDate?: Date) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getAttendanceHistory(startDate, endDate);
      attendance.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStudentGroups(studentId: number) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getGroupDetails(studentId);
      groups.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getAttendanceStats(studentId: number): Promise<StudentStats> {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getDashboardStats();
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getAbsencePercentage(groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getAbsencePercentage(groupId);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  // New card management functions
  async function assignCard(studentId: number, cardId: string) {
    isLoading.value = true;
    try {
      await studentApi.assignCard(studentId, cardId);
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
      await studentApi.removeCard(studentId);
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
    updateStudent,
    fetchStudentById,
    fetchStudentAttendance,
    fetchStudentGroups,
    getAbsencePercentage,
    getAttendanceStats,
    assignCard,
    removeCard
  };
});