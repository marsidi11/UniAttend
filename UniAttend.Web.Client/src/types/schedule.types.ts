import type { BaseEntity } from './base.types';

export interface Schedule extends BaseEntity {
  groupId: number;
  classroomId: number;
  dayOfWeek: number;
  startTime: string;
  endTime: string;
  groupName?: string;
  classroomName?: string;
}