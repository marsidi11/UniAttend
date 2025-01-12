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
  /** @format date-time */
  checkInTime?: string;
  checkInMethod?: string | null;
  isConfirmed?: boolean;
  courseName?: string | null;
  professor?: string | null;
}

export interface AttendanceReportDto {
  /** @format date-time */
  startDate?: string;
  /** @format date-time */
  endDate?: string;
  /** @format int32 */
  totalStudents?: number;
  /** @format int32 */
  totalClasses?: number;
  /** @format double */
  overallAttendance?: number;
  dailyRecords?: DailyAttendanceDto[] | null;
}

export interface AttendanceStatsDto {
  /** @format int32 */
  totalClasses?: number;
  /** @format int32 */
  attendedClasses?: number;
  /** @format double */
  attendanceRate?: number;
}

export interface ChangePasswordCommand {
  /** @format int32 */
  userId?: number;
  currentPassword?: string | null;
  newPassword?: string | null;
}

export interface ClassDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  groupId?: number;
  groupName?: string | null;
  /** @format int32 */
  classroomId?: number;
  classroomName?: string | null;
  /** @format date-time */
  date?: string;
  startTime?: TimeSpan;
  endTime?: TimeSpan;
  status?: string | null;
}

export interface ClassroomDto {
  /** @format int32 */
  id?: number;
  name?: string | null;
  readerDeviceId?: string | null;
  status?: string | null;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
}

export interface CreateAcademicYearCommand {
  name?: string | null;
  /** @format date-time */
  startDate?: string;
  /** @format date-time */
  endDate?: string;
}

export interface CreateClassroomCommand {
  name?: string | null;
  readerDeviceId?: string | null;
}

export interface CreateDepartmentCommand {
  name?: string | null;
}

export interface CreateGroupCommand {
  name?: string | null;
  /** @format int32 */
  subjectId?: number;
  /** @format int32 */
  academicYearId?: number;
  /** @format int32 */
  professorId?: number;
}

export interface CreateScheduleCommand {
  /** @format int32 */
  groupId?: number;
  /** @format int32 */
  classroomId?: number;
  /** @format int32 */
  dayOfWeek?: number;
  startTime?: TimeSpan;
  endTime?: TimeSpan;
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
  /** @format int32 */
  departmentId?: number;
}

export interface DailyAttendanceDto {
  /** @format date-time */
  date?: string;
  /** @format int32 */
  totalClasses?: number;
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
  groups?: GroupSummaryDto[] | null;
}

export interface EnrollStudentsCommand {
  /** @format int32 */
  groupId?: number;
  studentIds?: number[] | null;
}

export interface GenerateOtpRequest {
  /** @format int32 */
  classId?: number;
  /** @format int32 */
  studentId?: number;
}

export interface GroupReportDto {
  /** @format int32 */
  groupId?: number;
  groupName?: string | null;
  subjectName?: string | null;
  professorName?: string | null;
  /** @format int32 */
  totalStudents?: number;
  /** @format int32 */
  totalClasses?: number;
  /** @format double */
  averageAttendance?: number;
  students?: StudentAttendanceDto[] | null;
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

export interface GroupSummaryDto {
  /** @format int32 */
  groupId?: number;
  groupName?: string | null;
  subjectName?: string | null;
  /** @format int32 */
  enrolledStudents?: number;
  /** @format double */
  attendanceRate?: number;
}

export interface LoginCommand {
  username?: string | null;
  password?: string | null;
}

export interface OpenClassCommand {
  /** @format int32 */
  groupId?: number;
  /** @format int32 */
  classroomId?: number;
  /** @format int32 */
  courseId?: number;
  /** @format date-time */
  date?: string;
  startTime?: TimeSpan;
  endTime?: TimeSpan;
}

export interface OtpCode {
  /** @format int32 */
  id?: number;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
  /** @format int32 */
  studentId?: number;
  /** @format int32 */
  classId?: number;
  code?: string | null;
  /** @format date-time */
  expiryTime?: string;
  isUsed?: boolean;
}

export interface OtpValidationResponse {
  isValid?: boolean;
  message?: string | null;
  attendanceRecord?: AttendanceRecordDto;
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

export interface RecordCardAttendanceCommand {
  cardId?: string | null;
  deviceId?: string | null;
  /** @format int32 */
  classId?: number;
}

export interface RecordOtpAttendanceCommand {
  otpCode?: string | null;
  /** @format int32 */
  studentId?: number;
  /** @format int32 */
  classId?: number;
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
  /** @format int32 */
  userId?: number;
  newPassword?: string | null;
}

export interface ScheduleDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  groupId?: number;
  groupName?: string | null;
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

export interface StudentAttendanceDto {
  /** @format int32 */
  studentId?: number;
  studentNumber?: string | null;
  fullName?: string | null;
  /** @format int32 */
  attendedClasses?: number;
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
  totalClasses?: number;
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

export interface SubjectAttendanceDto {
  /** @format int32 */
  subjectId?: number;
  subjectName?: string | null;
  groupName?: string | null;
  /** @format int32 */
  attendedClasses?: number;
  /** @format int32 */
  totalClasses?: number;
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

export interface UpdateGroupCommand {
  /** @format int32 */
  id?: number;
  name?: string | null;
  /** @format int32 */
  subjectId?: number;
  /** @format int32 */
  professorId?: number;
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
  groupId?: number;
  /** @format int32 */
  classroomId?: number;
  /** @format int32 */
  dayOfWeek?: number;
  startTime?: TimeSpan;
  endTime?: TimeSpan;
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
  /** @format int32 */
  departmentId?: number | null;
  departmentName?: string | null;
  isActive?: boolean;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
}

export interface UserGroupDto {
  /** @format int32 */
  groupId?: number;
  groupName?: string | null;
  subjectName?: string | null;
  academicYearName?: string | null;
}

export interface UserProfileDto {
  /** @format int32 */
  id?: number;
  username?: string | null;
  email?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  role?: UserRole;
}

/** @format int32 */
export enum UserRole {
  Value1 = 1,
  Value2 = 2,
  Value3 = 3,
  Value4 = 4,
}

export interface ValidateOtpRequest {
  code?: string | null;
  /** @format int32 */
  classId?: number;
}
