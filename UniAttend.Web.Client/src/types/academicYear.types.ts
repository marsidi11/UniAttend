import type { ActiveEntity } from './base.types';

export interface AcademicYear extends ActiveEntity {
  name: string;
  startDate: Date;
  endDate: Date;
  studyGroups?: StudyGroup[];
}