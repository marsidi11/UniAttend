import type { BaseEntity, Role } from './base.types';

export interface User extends BaseEntity {
  username: string;
  email: string;
  role: Role;
  firstName: string;
  lastName: string;
  isActive: boolean;
}

export interface Student extends User {
  studentId: string;
  cardId?: string;
  departmentId: number;
  departmentName?: string;
}

export interface Professor extends User {
  departmentId: number;
  departmentName?: string;
}