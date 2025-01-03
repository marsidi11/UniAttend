export interface LoginCredentials {
  username: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  userId: number;
  role: string;
}

export interface UserProfile {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: 'student' | 'professor' | 'admin' | 'secretary';
}

export interface RegisterCredentials {
    username: string;
    password: string;
    email: string;
    firstName: string;
    lastName: string;
    role: 'student' | 'professor' | 'admin' | 'secretary';
    studentId?: string;
    departmentId?: number;
    cardId?: string;
  }
  
  export interface ResetPasswordCredentials {
    email: string;
    token: string;
    newPassword: string;
  }