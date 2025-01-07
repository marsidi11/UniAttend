export interface TableItem {
  id: number;
  [key: string]: any;
}

export interface Column<T extends TableItem> {
  key: keyof T;
  label: string;
  render?: (value: T[keyof T]) => string | number;
}

export interface Action<T = any> {
  label: string;
  icon?: string;
  action: (item: T) => void;
}