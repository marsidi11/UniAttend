import type { ActiveEntity } from './base.types';

export interface Subject extends ActiveEntity {
    name: string;
    departmentId: number;
    description?: string;
    isActive: boolean;
  }