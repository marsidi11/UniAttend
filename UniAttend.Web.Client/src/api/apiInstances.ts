import { createApiConfig } from './clientConfig';
import { Auth } from './generated/Auth';
import { User } from './generated/User';
import { Student } from './generated/Student'; 
import { Classes } from './generated/Classes';
import { Otp } from './generated/Otp';
import { Reports } from './generated/Reports';

// Create single config instance
const config = createApiConfig();

// Initialize all API instances with the same config
export const authApi = new Auth(config);
export const userApi = new User(config);
export const studentApi = new Student(config);
export const classesApi = new Classes(config);
export const otpApi = new Otp(config);
export const reportsApi = new Reports(config);