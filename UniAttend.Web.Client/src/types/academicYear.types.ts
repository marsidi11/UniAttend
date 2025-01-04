import type { ActiveEntity } from './base.types';
import type { StudyGroup } from './group.types';

export interface AcademicYear extends ActiveEntity {
  name: string;
  startDate: Date;
  endDate: Date;
  studyGroups?: StudyGroup[];
}

export interface CreateAcademicYearRequest {
  name: string;
  startDate: Date;
  endDate: Date;
  isActive?: boolean;
}

export interface UpdateAcademicYearRequest {
  name?: string;
  startDate?: Date;
  endDate?: Date;
  isActive?: boolean;
}

export interface AcademicYearResponse extends AcademicYear {
  totalGroups: number;
  totalStudents: number;
}