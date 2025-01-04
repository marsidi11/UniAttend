import type { BaseEntity } from './base.types';
import type { Schedule } from './schedule.types';

export interface Classroom extends BaseEntity {
  name: string;
  readerDeviceId?: string;
  capacity?: number;
  location?: string;
  isAvailable: boolean;
  schedule?: Schedule[];
}

export interface ReaderDevice {
  id: string;
  name: string;
  status: 'Active' | 'Inactive';
  lastPing?: Date;
  classroomId?: number;
  classroomName?: string;
}

export interface ClassroomSchedule extends Schedule {
  groupName: string;
  subjectName: string;
  professorName: string;
}

export interface ReaderDeviceStatus {
  deviceId: string;
  isOnline: boolean;
  lastPingTime: Date;
  batteryLevel?: number;
}