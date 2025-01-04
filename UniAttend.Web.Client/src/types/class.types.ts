import type { BaseEntity } from './base.types';

export interface Class extends BaseEntity {
  groupId: number;
  classroomId: number;
  date: Date;
  startTime: string;
  endTime: string;
  status: 'Active' | 'Completed';
}