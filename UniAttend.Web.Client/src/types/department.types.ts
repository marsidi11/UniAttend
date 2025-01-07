import type { ActiveEntity } from './base.types';
import type { Student } from './student.types';
import type { Professor } from './professor.types';

export interface Department extends ActiveEntity {
  name: string;
  subjects?: Subject[];
  students?: Student[];
  professors?: Professor[];
  subjectsCount?: number;
  studentsCount?: number;
  professorsCount?: number;
}

export interface Subject extends ActiveEntity {
  name: string;
  description?: string;
  credits: number;
  departmentId: number;
}

export interface CreateDepartmentRequest {
  name: string;
}

export interface UpdateDepartmentRequest {
  name: string;
  isActive: boolean;
}