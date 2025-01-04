import type { BaseEntity } from './base.types';

export interface StudyGroup extends BaseEntity {
  name: string;
  subjectId: number;
  academicYearId: number;
  professorId: number;
  subject?: string;
  professor?: string;
  studentCount?: number;
}

export interface GroupStudent extends BaseEntity {
  groupId: number;
  studentId: number;
  studentName?: string;
}