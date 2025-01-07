import type { BaseEntity } from './base.types';

export interface StudyGroup extends BaseEntity {
  name: string;
  subjectId: number;
  subjectName?: string;
  academicYearId: number;
  professorId: number;
  professorName?: string;
  studentCount?: number;
  studentsCount?: number;
  averageAttendance?: number;
  classesCount?: number;
  isActive: boolean;
}

export interface GroupStudent extends BaseEntity {
  groupId: number;
  studentId: number;
  fullName?: string;
  studentName?: string;
  attendanceRate?: number;
}