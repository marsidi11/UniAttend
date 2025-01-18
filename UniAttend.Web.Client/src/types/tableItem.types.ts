import { StringRole } from './base.types';

export interface TableItem {
  id: number;
  [key: string]: any;
}

export interface Column<T extends TableItem> {
  key: keyof T;
  label: string;
  sortable?: boolean;
  cellClass?: (value: any) => string;
  render?: (value: T[keyof T]) => string | number;
}

export interface Action<T = any> {
  label: string;
  icon?: string;
  action: (item: T, event?: Event) => void;
}

export interface StaffTableItem extends TableItem {
  id: number;
  username?: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  role?: StringRole;
  departmentId?: number;
  departmentName?: string;
  isActive?: boolean;
  createdAt?: string;
  updatedAt?: string;
}