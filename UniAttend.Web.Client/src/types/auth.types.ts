import type { Role, BaseEntity } from './base.types';

export interface User extends BaseEntity {
  username: string;
  email: string;
  role: Role;
  firstName: string;
  lastName: string;
  isActive: boolean;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
  email: string;
  firstName: string;
  lastName: string;
  role: Role;
}