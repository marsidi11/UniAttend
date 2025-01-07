import type { ActiveEntity } from './base.types';

export interface Subject extends ActiveEntity {
  name: string;
  departmentId: number;
  departmentName?: string;
  description?: string;
  isActive: boolean;
  groupsCount?: number;
  studentsCount?: number;
  averageAttendance?: number;
}

export interface SubjectDetails extends Subject {
  groups?: StudyGroup[];
}