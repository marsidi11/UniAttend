import { createApiConfig } from './clientConfig';
import { Auth } from './generated/Auth';
import { User } from './generated/User';
import { Student } from './generated/Student';
// import { Professor } from './generated/Professor';
import { Classes } from './generated/Classes';
import { StudyGroups } from './generated/studyGroups';
import { Classrooms } from './generated/Classrooms';
import { Departments } from './generated/Departments';
import { Subjects } from './generated/Subjects';
import { Schedule } from './generated/Schedule';
import { AcademicYear } from './generated/AcademicYear';
import { Attendance } from './generated/Attendance';
import { Otp } from './generated/Otp';
import { Reports } from './generated/Reports';
// import { Cards } from './generated/Cards';

// Create single config instance
const config = createApiConfig();

// Initialize all API instances with the same config
export const authApi = new Auth(config);
export const userApi = new User(config);
export const studentApi = new Student(config);
// export const professorApi = new Professor(config);
export const classApi = new Classes(config);
export const studyGroupApi = new StudyGroups(config);
export const classroomApi = new Classrooms(config);
export const departmentApi = new Departments(config);
export const subjectApi = new Subjects(config);
export const scheduleApi = new Schedule(config);
export const academicYearApi = new AcademicYear(config);
export const attendanceApi = new Attendance(config);
export const otpApi = new Otp(config);
export const reportApi = new Reports(config);
// export const cardApi = new Cards(config);