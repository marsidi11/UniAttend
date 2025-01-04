import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { 
  Student, 
  StudentAttendance, 
  StudentGroup 
} from '@/types/student.types';
import { studentApi } from '@/api/endpoints/studentApi';

export const useStudentStore = defineStore('student', () => {
  // State
  const students = ref<Student[]>([]);
  const currentStudent = ref<Student | null>(null);
  const attendance = ref<StudentAttendance[]>([]);
  const groups = ref<StudentGroup[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Getters
  const activeStudents = computed(() => 
    students.value.filter(s => s.isActive)
  );

  const groupedByDepartment = computed(() => {
    const grouped = new Map();
    students.value.forEach(student => {
      const dept = student.departmentName;
      if (!grouped.has(dept)) {
        grouped.set(dept, []);
      }
      grouped.get(dept).push(student);
    });
    return grouped;
  });

  // Actions
  async function fetchStudents(departmentId?: number) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getAll(departmentId);
      students.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStudentById(id: number) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getById(id);
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
      const { data } = await studentApi.getAttendance(studentId, startDate, endDate);
      attendance.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchStudentGroups(studentId: number) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getGroups(studentId);
      groups.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function checkAbsenceStatus(studentId: number, groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getAbsenceStatus(studentId, groupId);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  } 
  
  async function getAttendancePercentage(studentId: number, groupId: number) {
    isLoading.value = true;
    try {
      const { data } = await studentApi.getAttendancePercentage(studentId, groupId);
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
    fetchStudents,
    fetchStudentById,
    fetchStudentAttendance,
    fetchStudentGroups,
    checkAbsenceStatus,
    getAttendancePercentage
  };
});