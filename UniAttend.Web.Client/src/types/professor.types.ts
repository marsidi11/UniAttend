import type { User } from './user.types';
import type { Course } from './course.types';

export interface Professor extends User {
  departmentId: number;
  departmentName?: string;
}

export interface ProfessorSchedule {
  id: number;
  groupId: number;
  classroomId: number;
  dayOfWeek: number;
  startTime: string;
  endTime: string;
  groupName: string;
  classroomName: string;
}