using Microsoft.AspNetCore.Hosting;
using FastReport.Export.PdfSimple;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using System.Data;

namespace UniAttend.Infrastructure.Services
{
    /// <summary>
    /// Service for generating professional PDF reports using FastReport.NET
    /// </summary>
    public class FastReportService : IReportService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly string _reportsPath;

        /// <summary>
        /// Initializes a new instance of the FastReportService
        /// </summary>
        public FastReportService(
            IWebHostEnvironment environment,
            IAttendanceRecordRepository attendanceRepository,
            IStudentRepository studentRepository,
            IStudyGroupRepository studyGroupRepository,
            IDepartmentRepository departmentRepository)
        {
            _environment = environment;
            _attendanceRepository = attendanceRepository;
            _studentRepository = studentRepository;
            _studyGroupRepository = studyGroupRepository;
            _departmentRepository = departmentRepository;
            _reportsPath = Path.Combine(_environment.ContentRootPath, "Reports");
        }

        /// <summary>
        /// Generates a PDF report for student attendance
        /// </summary>
        public async Task<byte[]> GenerateStudentReportAsync(int studentId, DateTime? startDate, DateTime? endDate)
        {
            var report = new FastReport.Report();
            report.Load(Path.Combine(_reportsPath, "StudentReport.frx"));

            var student = await _studentRepository.GetByIdAsync(studentId, CancellationToken.None);
            var attendance = await _attendanceRepository.GetDetailedStudentAttendanceAsync(studentId, startDate, endDate, CancellationToken.None);

            var dataSet = new DataSet();
            var dataTable = new DataTable("AttendanceRecords");
            // Add columns
            dataTable.Columns.Add("Date", typeof(DateTime));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));

            // Populate data from attendance records
            foreach (var record in attendance)
            {
                dataTable.Rows.Add(
                    record.CourseSession.Date,
                    record.CourseSession.StudyGroup.Subject.Name,
                    record.IsConfirmed ? "Present" : "Absent"
                );
            }
            dataSet.Tables.Add(dataTable);

            report.SetParameterValue("StudentName", $"{student.User.FirstName} {student.User.LastName}");
            report.SetParameterValue("StudentId", student.StudentId);
            report.SetParameterValue("DateRange", $"{startDate:d} - {endDate:d}");

            report.RegisterData(dataSet);
            report.Prepare();

            using var ms = new MemoryStream();
            report.Export(new PDFSimpleExport(), ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Generates a PDF report for group attendance
        /// </summary>
        public async Task<byte[]> GenerateGroupReportAsync(int groupId, DateTime? startDate, DateTime? endDate)
        {
            var report = new FastReport.Report();
            report.Load(Path.Combine(_reportsPath, "GroupReport.frx"));

            var group = await _studyGroupRepository.GetByIdWithDetailsAsync(groupId, CancellationToken.None);
            var attendance = await _attendanceRepository.GetDetailedByCourseSessionIdAsync(groupId, CancellationToken.None);

            var dataSet = new DataSet();
            var dataTable = new DataTable("AttendanceRecords");
            dataTable.Columns.Add("Date", typeof(DateTime));
            dataTable.Columns.Add("StudentName", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("AttendanceRate", typeof(decimal));

            foreach (var record in attendance)
            {
                dataTable.Rows.Add(
                    record.CourseSession.Date,
                    $"{record.Student.User.FirstName} {record.Student.User.LastName}",
                    record.IsConfirmed ? (record.IsAbsent ? "Absent" : "Present") : "Pending",
                    record.Status
                );
            }
            dataSet.Tables.Add(dataTable);

            report.SetParameterValue("GroupName", group.Name);
            report.SetParameterValue("Subject", group.Subject.Name);
            report.SetParameterValue("DateRange", $"{startDate:d} - {endDate:d}");

            report.RegisterData(dataSet);
            report.Prepare();

            using var ms = new MemoryStream();
            report.Export(new PDFSimpleExport(), ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Generates a PDF report for department attendance
        /// </summary>
        public async Task<byte[]> GenerateDepartmentReportAsync(int departmentId, int? academicYearId)
        {
            var report = new FastReport.Report();
            report.Load(Path.Combine(_reportsPath, "DepartmentReport.frx"));

            var department = await _departmentRepository.GetByIdAsync(departmentId, CancellationToken.None);
            var stats = await _attendanceRepository.GetDepartmentAttendanceStatsAsync(departmentId, academicYearId, CancellationToken.None);

            var dataSet = new DataSet();
            var dataTable = new DataTable("DepartmentStats");
            dataTable.Columns.Add("TotalStudents", typeof(int));
            dataTable.Columns.Add("AttendanceRate", typeof(decimal));
            dataTable.Columns.Add("PresentCount", typeof(int));
            dataTable.Columns.Add("AbsentCount", typeof(int));
            dataTable.Columns.Add("PendingCount", typeof(int));

            // Add a single row with department stats
            dataTable.Rows.Add(
                stats.TotalStudents,
                stats.AttendanceRate,
                stats.PresentCount,
                stats.AbsentCount,
                stats.PendingCount
            );

            dataSet.Tables.Add(dataTable);

            report.SetParameterValue("DepartmentName", department.Name);
            report.SetParameterValue("AcademicYear", academicYearId.HasValue ? $"Academic Year: {academicYearId}" : "All Years");
            report.SetParameterValue("AverageAttendance", stats.AverageAttendance);
            report.SetParameterValue("TotalSessions", stats.TotalSessions);

            report.RegisterData(dataSet);
            report.Prepare();

            using var ms = new MemoryStream();
            report.Export(new PDFSimpleExport(), ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Generates a detailed attendance report for a study group
        /// </summary>
        public async Task<byte[]> GenerateAttendanceReportAsync(int studyGroupId, DateTime startDate, DateTime endDate)
        {
            var report = new FastReport.Report();
            report.Load(Path.Combine(_reportsPath, "AttendanceReport.frx"));

            var records = await _attendanceRepository.GetDetailedByCourseSessionIdAsync(studyGroupId, CancellationToken.None);
            var group = await _studyGroupRepository.GetByIdWithDetailsAsync(studyGroupId, CancellationToken.None);

            var dataSet = new DataSet();
            var dataTable = new DataTable("AttendanceRecords");
            dataTable.Columns.Add("Date", typeof(DateTime));
            dataTable.Columns.Add("StudentName", typeof(string));
            dataTable.Columns.Add("Present", typeof(bool));
            dataTable.Columns.Add("Absent", typeof(bool));

            foreach (var record in records)
            {
                dataTable.Rows.Add(
                    record.CourseSession.Date,
                    $"{record.Student.User.FirstName} {record.Student.User.LastName}",
                    record.IsConfirmed && !record.IsAbsent,
                    record.IsConfirmed && record.IsAbsent
                );
            }
            dataSet.Tables.Add(dataTable);

            report.SetParameterValue("GroupName", group.Name);
            report.SetParameterValue("Subject", group.Subject.Name);
            report.SetParameterValue("DateRange", $"{startDate:d} - {endDate:d}");
            report.SetParameterValue("Professor", $"{group.Professor.User.FirstName} {group.Professor.User.LastName}");

            report.RegisterData(dataSet);
            report.Prepare();

            using var ms = new MemoryStream();
            report.Export(new PDFSimpleExport(), ms);
            return ms.ToArray();
        }
    }
}