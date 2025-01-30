namespace UniAttend.Application.Features.Reports.DTOs
{
    public record GroupReportDto
    {
        public int StudyGroupId { get; init; }
        public string StudyGroupName { get; init; } = string.Empty;
        public string SubjectName { get; init; } = string.Empty;
        public string ProfessorName { get; init; } = string.Empty;
        public int TotalStudents { get; init; }
        public int TotalCourseSessions { get; init; }
        public decimal AverageAttendance { get; init; }
        public List<AttendanceRecordDto> Students { get; init; } = new();
    }

    public record AttendanceRecordDto
    {
        public int StudentId { get; init; }
        public string StudentNumber { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public int AttendedCourseSessions { get; init; }
        public decimal AttendanceRate { get; init; }
    }
}