export interface AttendanceStats {
  totalStudents: number;
  presentToday: number;
  attendanceRate: number;
}

export interface AttendanceRecord {
  id: number;
  checkInTime: Date;
  checkInMethod: string;
  isConfirmed: boolean;
  courseName: string;
  professorName: string;
}

export interface ClassAttendanceResponse {
  records: AttendanceRecord[];
  stats: AttendanceStats;
}