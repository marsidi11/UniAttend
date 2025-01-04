import type { BaseEntity } from './base.types';

export interface Schedule extends BaseEntity {
  groupId: number;
  classroomId: number;
  dayOfWeek: number;
  startTime: string;
  endTime: string;
  groupName?: string;
  classroomName?: string;
  professorName?: string;
  subjectName?: string;
}

export interface CreateScheduleRequest {
  groupId: number;
  classroomId: number;
  dayOfWeek: number;
  startTime: string;
  endTime: string;
}

export interface UpdateScheduleRequest {
  groupId?: number;
  classroomId?: number;
  dayOfWeek?: number;
  startTime?: string;
  endTime?: string;
}

export interface ScheduleConflictRequest {
  classroomId: number;
  dayOfWeek: number;
  startTime: string;
  endTime: string;
  excludeScheduleId?: number;
}