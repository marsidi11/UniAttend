export interface BaseEntity {
  id: number;
  createdAt: Date;
  updatedAt?: Date;
}

export interface ActiveEntity extends BaseEntity {
  isActive: boolean;
}

export type Role = 'Admin' | 'Secretary' | 'Professor' | 'Student';