/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface AcademicYearDto {
  /** @format int32 */
  id?: number;
  name?: string | null;
  /** @format date-time */
  startDate?: string;
  /** @format date-time */
  endDate?: string;
  isActive?: boolean;
  /** @format int32 */
  totalGroups?: number;
  /** @format int32 */
  totalStudents?: number;
}

export interface AcademicYearReportDto {
  /** @format int32 */
  academicYearId?: number;
  name?: string | null;
  /** @format date-time */
  startDate?: string;
  /** @format date-time */
  endDate?: string;
  /** @format int32 */
  totalStudents?: number;
  /** @format int32 */
  totalGroups?: number;
  /** @format int32 */
  activeGroups?: number;
  /** @format double */
  overallAttendance?: number;
  /** @format int32 */
  pendingAttendanceConfirmations?: number;
}

export interface AssignCardCommand {
  /** @format int32 */
  studentId?: number;
  cardId?: string | null;
}

export interface AssignReaderCommand {
  /** @format int32 */
  classroomId?: number;
  readerDeviceId?: string | null;
}

export interface AttendanceRecordDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  courseSessionId?: number;
  /** @format int32 */
  studentId?: number;
  studentName?: string | null;
  /** @format date-time */
  checkInTime?: string;
  checkInMethod?: CheckInMethod;
  isConfirmed?: boolean;
  /** @format date-time */
  confirmationTime?: string | null;
  studyGroupName?: string | null;
  classroomName?: string | null;
  sessionStartTime?: TimeSpan;
  sessionEndTime?: TimeSpan;
}

export interface AttendanceReportDto {
  /** @format date-time */
  startDate?: string;
  /** @format date-time */
  endDate?: string;
  /** @format int32 */
  totalStudents?: number;
  /** @format int32 */
  totalCourseSessions?: number;
  /** @format double */
  overallAttendance?: number;
  dailyRecords?: DailyAttendanceDto[] | null;
}

export interface AttendanceStatsDto {
  /** @format int32 */
  totalCourseSessions?: number;
  /** @format int32 */
  attendedCourseSessions?: number;
  /** @format double */
  attendanceRate?: number;
}

export interface AuthResult {
  accessToken?: string | null;
  refreshToken?: string | null;
  /** @format date-time */
  expiresAt?: string;
  user?: UserAuthDto;
}

export interface ChangePasswordCommand {
  /** @format int32 */
  userId?: number;
  currentPassword?: string | null;
  newPassword?: string | null;
}

/** @format int32 */
export enum CheckInMethod {
  Value0 = 0,
  Value1 = 1,
  Value2 = 2,
}

export interface ClassroomDto {
  /** @format int32 */
  id?: number;
  name?: string | null;
  readerDeviceId?: string | null;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
}

export interface CourseSessionDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  studyGroupId?: number;
  studyGroupName?: string | null;
  /** @format int32 */
  classroomId?: number;
  classroomName?: string | null;
  /** @format date-time */
  date?: string;
  startTime?: TimeSpan;
  endTime?: TimeSpan;
  status?: string | null;
}

export interface CreateAcademicYearCommand {
  name?: string | null;
  /** @format date-time */
  startDate?: string;
  /** @format date-time */
  endDate?: string;
  isActive?: boolean;
}

export interface CreateClassroomCommand {
  name?: string | null;
  readerDeviceId?: string | null;
}

export interface CreateDepartmentCommand {
  name?: string | null;
}

export interface CreateScheduleCommand {
  /** @format int32 */
  studyGroupId?: number;
  /** @format int32 */
  classroomId?: number;
  /** @format int32 */
  dayOfWeek?: number;
  startTime?: TimeSpan;
  endTime?: TimeSpan;
}

export interface CreateStudyGroupCommand {
  name?: string | null;
  /** @format int32 */
  subjectId?: number;
  /** @format int32 */
  academicYearId?: number;
  /** @format int32 */
  professorId?: number;
}

export interface CreateSubjectCommand {
  name?: string | null;
  description?: string | null;
  /** @format int32 */
  departmentId?: number;
  /** @format int32 */
  credits?: number;
}

export interface CreateUserCommand {
  username?: string | null;
  email?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  role?: UserRole;
  departmentIds?: number[] | null;
}

export interface DailyAttendanceDto {
  /** @format date-time */
  date?: string;
  /** @format int32 */
  totalCourseSessions?: number;
  /** @format int32 */
  presentStudents?: number;
  /** @format int32 */
  absentStudents?: number;
  /** @format double */
  attendanceRate?: number;
}

export interface DepartmentDto {
  /** @format int32 */
  id?: number;
  name?: string | null;
  isActive?: boolean;
  /** @format int32 */
  subjectsCount?: number;
  /** @format int32 */
  studentsCount?: number;
  /** @format int32 */
  professorsCount?: number;
  /** @format date-time */
  createdAt?: string;
}

export interface DepartmentReportDto {
  /** @format int32 */
  departmentId?: number;
  departmentName?: string | null;
  /** @format int32 */
  totalStudents?: number;
  /** @format int32 */
  totalGroups?: number;
  /** @format int32 */
  totalSubjects?: number;
  /** @format double */
  averageAttendance?: number;
  groups?: StudyGroupSummaryDto[] | null;
}

export interface EnrollStudentsCommand {
  /** @format int32 */
  studyGroupId?: number;
  studentIds?: number[] | null;
}

export interface GroupReportDto {
  /** @format int32 */
  studyGroupId?: number;
  studyGroupName?: string | null;
  subjectName?: string | null;
  professorName?: string | null;
  /** @format int32 */
  totalStudents?: number;
  /** @format int32 */
  totalCourseSessions?: number;
  /** @format double */
  averageAttendance?: number;
  students?: AttendanceRecordDto[] | null;
}

export interface GroupStudentDto {
  /** @format int32 */
  studentId?: number;
  studentName?: string | null;
  studentNumber?: string | null;
  /** @format double */
  attendanceRate?: number;
  isActive?: boolean;
}

export interface LoginCommand {
  username?: string | null;
  password?: string | null;
}

export interface OpenCourseSessionCommand {
  /** @format int32 */
  studyGroupId?: number;
  /** @format int32 */
  classroomId?: number;
  /** @format int32 */
  courseSessionId?: number;
  /** @format date-time */
  date?: string;
  startTime?: TimeSpan;
  endTime?: TimeSpan;
}

export interface ProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  [key: string]: any;
}

export interface ProfessorDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  userId?: number;
  fullName?: string | null;
  email?: string | null;
  departments?: DepartmentDto[] | null;
  isActive?: boolean;
}

export interface RecordCardAttendanceCommand {
  cardId?: string | null;
  deviceId?: string | null;
  /** @format int32 */
  courseSessionId?: number;
}

export interface RecordOtpAttendanceCommand {
  otpCode?: string | null;
  /** @format int32 */
  studentId?: number;
  /** @format int32 */
  courseSessionId?: number;
  verificationType?: VerificationType;
}

export interface RefreshTokenCommand {
  accessToken?: string | null;
  refreshToken?: string | null;
}

export interface RegisterStudentCommand {
  studentId?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  email?: string | null;
  /** @format int32 */
  departmentId?: number;
  cardId?: string | null;
}

export interface ResetPasswordCommand {
  email?: string | null;
}

export interface ScheduleDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  studyGroupId?: number;
  studyGroupName?: string | null;
  /** @format int32 */
  classroomId?: number;
  classroomName?: string | null;
  /** @format int32 */
  dayOfWeek?: number;
  startTime?: TimeSpan;
  endTime?: TimeSpan;
  subjectName?: string | null;
  professorName?: string | null;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
}

export interface AttendanceRecordDto {
  /** @format int32 */
  studentId?: number;
  studentNumber?: string | null;
  fullName?: string | null;
  /** @format int32 */
  attendedCourseSessions?: number;
  /** @format double */
  attendanceRate?: number;
}

export interface StudentReportDto {
  /** @format int32 */
  studentId?: number;
  studentNumber?: string | null;
  fullName?: string | null;
  departmentName?: string | null;
  cardId?: string | null;
  /** @format int32 */
  totalAttendance?: number;
  /** @format int32 */
  totalCourseSessions?: number;
  /** @format double */
  attendanceRate?: number;
  subjects?: SubjectAttendanceDto[] | null;
}

export interface StudyGroupDto {
  /** @format int32 */
  id?: number;
  name?: string | null;
  /** @format int32 */
  subjectId?: number;
  subjectName?: string | null;
  /** @format int32 */
  academicYearId?: number;
  academicYearName?: string | null;
  /** @format int32 */
  professorId?: number;
  professorName?: string | null;
  /** @format int32 */
  studentsCount?: number;
  /** @format double */
  attendanceRate?: number;
  isActive?: boolean;
}

export interface StudyGroupSummaryDto {
  /** @format int32 */
  studyGroupId?: number;
  studyGroupName?: string | null;
  subjectName?: string | null;
  /** @format int32 */
  enrolledStudents?: number;
  /** @format double */
  attendanceRate?: number;
}

export interface SubjectAttendanceDto {
  /** @format int32 */
  subjectId?: number;
  subjectName?: string | null;
  studyGroupName?: string | null;
  /** @format int32 */
  attendedCourseSessions?: number;
  /** @format int32 */
  totalCourseSessions?: number;
  /** @format double */
  attendanceRate?: number;
}

export interface SubjectDto {
  /** @format int32 */
  id?: number;
  name?: string | null;
  description?: string | null;
  /** @format int32 */
  departmentId?: number;
  departmentName?: string | null;
  /** @format int32 */
  credits?: number;
  isActive?: boolean;
  /** @format int32 */
  groupsCount?: number;
  /** @format int32 */
  studentsCount?: number;
  /** @format double */
  averageAttendance?: number;
}

export interface TimeSpan {
  /** @format int64 */
  ticks?: number;
  /** @format int32 */
  days?: number;
  /** @format int32 */
  hours?: number;
  /** @format int32 */
  milliseconds?: number;
  /** @format int32 */
  microseconds?: number;
  /** @format int32 */
  nanoseconds?: number;
  /** @format int32 */
  minutes?: number;
  /** @format int32 */
  seconds?: number;
  /** @format double */
  totalDays?: number;
  /** @format double */
  totalHours?: number;
  /** @format double */
  totalMilliseconds?: number;
  /** @format double */
  totalMicroseconds?: number;
  /** @format double */
  totalNanoseconds?: number;
  /** @format double */
  totalMinutes?: number;
  /** @format double */
  totalSeconds?: number;
}

export interface TotpSetupDto {
  secretKey?: string | null;
  qrCodeUri?: string | null;
}

export interface TransferStudentCommand {
  /** @format int32 */
  studentId?: number;
  /** @format int32 */
  fromGroupId?: number;
  /** @format int32 */
  toGroupId?: number;
}

export interface UpdateAcademicYearCommand {
  /** @format int32 */
  id?: number;
  name?: string | null;
  /** @format date-time */
  startDate?: string | null;
  /** @format date-time */
  endDate?: string | null;
  isActive?: boolean | null;
}

export interface UpdateClassroomCommand {
  /** @format int32 */
  id?: number;
  name?: string | null;
  readerDeviceId?: string | null;
}

export interface UpdateDepartmentCommand {
  /** @format int32 */
  id?: number;
  name?: string | null;
  description?: string | null;
  isActive?: boolean;
}

export interface UpdateProfileCommand {
  /** @format int32 */
  userId?: number;
  firstName?: string | null;
  lastName?: string | null;
  email?: string | null;
}

export interface UpdateScheduleCommand {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  studyGroupId?: number;
  /** @format int32 */
  classroomId?: number;
  /** @format int32 */
  dayOfWeek?: number;
  startTime?: TimeSpan;
  endTime?: TimeSpan;
}

export interface UpdateStudyGroupCommand {
  /** @format int32 */
  id?: number;
  name?: string | null;
  /** @format int32 */
  subjectId?: number;
  /** @format int32 */
  professorId?: number;
  isActive?: boolean;
}

export interface UpdateSubjectCommand {
  /** @format int32 */
  id?: number;
  name?: string | null;
  description?: string | null;
  /** @format int32 */
  credits?: number;
  isActive?: boolean;
}

export interface UpdateUserCommand {
  /** @format int32 */
  id?: number;
  email?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  /** @format int32 */
  departmentId?: number | null;
  isActive?: boolean;
}

export interface UserAuthDto {
  /** @format int32 */
  id?: number;
  username?: string | null;
  email?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  role?: UserRole;
}

export interface UserDetailsDto {
  /** @format int32 */
  id?: number;
  username?: string | null;
  email?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  role?: UserRole;
  /** @format int32 */
  departmentId?: number | null;
  departmentName?: string | null;
  isActive?: boolean;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
  attendanceStats?: AttendanceStatsDto;
  groups?: UserGroupDto[] | null;
}

export interface UserDto {
  /** @format int32 */
  id?: number;
  username?: string | null;
  email?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  role?: UserRole;
  departmentName?: string | null;
  departments?: DepartmentDto[] | null;
  isActive?: boolean;
  totpSecret?: string | null;
  isTwoFactorEnabled?: boolean | null;
  isTwoFactorVerified?: boolean | null;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
}

export interface UserGroupDto {
  /** @format int32 */
  studyGroupId?: number;
  studyGroupName?: string | null;
  subjectName?: string | null;
  academicYearName?: string | null;
  professorName?: string | null;
}

export interface UserProfileDto {
  /** @format int32 */
  id?: number;
  username?: string | null;
  email?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  role?: UserRole;
  isTwoFactorEnabled?: boolean;
  isTwoFactorVerified?: boolean;
}

/** @format int32 */
export enum UserRole {
  Value1 = 1,
  Value2 = 2,
  Value3 = 3,
  Value4 = 4,
}

/** @format int32 */
export enum VerificationType {
  Value0 = 0,
  Value1 = 1,
}
