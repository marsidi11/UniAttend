import type { BaseEntity } from './base.types';

export interface Classroom extends BaseEntity {
  name: string;
  readerDeviceId?: string;
}

export interface ReaderDevice {
  id: string;
  name: string;
  status: 'Active' | 'Inactive';
}