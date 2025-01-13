export interface BaseEntity {
  id: number;
  createdAt: Date;
  updatedAt?: Date;
}

export interface ActiveEntity extends BaseEntity {
  isActive: boolean;
}

// Numeric role type from API
export enum NumericRole {
  Admin = 1,
  Secretary = 2,
  Professor = 3,
  Student = 4
}

// String role type for frontend
export type StringRole = 'admin' | 'secretary' | 'professor' | 'student';

// Combined role type
export type Role = StringRole;

// Role mapping utility
export const roleMapping = {
  [NumericRole.Admin]: 'admin' as StringRole,
  [NumericRole.Secretary]: 'secretary' as StringRole,
  [NumericRole.Professor]: 'professor' as StringRole,
  [NumericRole.Student]: 'student' as StringRole
};

export const getRoleString = (numericRole: NumericRole): StringRole => {
  return roleMapping[numericRole] || 'admin';
};