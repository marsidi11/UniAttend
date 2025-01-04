export interface AttendanceRecord {
  id: number;
  studentId: number;
  classId: number;
  checkInTime: Date;
  checkInMethod: 'Card' | 'OTP';
  isConfirmed: boolean;
  studentName: string;
  courseName: string;
}

export interface AttendanceStats {
  totalStudents: number;
  presentToday: number;
  attendanceRate: number;
  absentStudents: number;
}

export interface ClassAttendance {
  id: number;
  groupId: number;
  classroomId: number;
  date: Date;
  startTime: string;
  endTime: string;
  status: 'Active' | 'Completed';
  records: AttendanceRecord[];
  stats: AttendanceStats;
}

export interface RecordCardAttendanceRequest {
  cardId: string;
  deviceId: string;
}

export interface RecordOtpAttendanceRequest {
  otpCode: string;
  classId: number;
}