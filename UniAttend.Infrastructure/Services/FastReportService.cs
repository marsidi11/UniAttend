using Microsoft.AspNetCore.Hosting;
using FastReport.Export.PdfSimple;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using System.Data;
using UniAttend.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using UniAttend.Core.Entities.Attendance;

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
        private readonly ILogger<FastReportService> _logger;

        /// <summary>
        /// Initializes a new instance of the FastReportService
        /// </summary>
        public FastReportService(
            IWebHostEnvironment environment,
            IAttendanceRecordRepository attendanceRepository,
            IStudentRepository studentRepository,
            IStudyGroupRepository studyGroupRepository,
            IDepartmentRepository departmentRepository,
            ILogger<FastReportService> logger)
        {
            _environment = environment;
            _attendanceRepository = attendanceRepository;
            _studentRepository = studentRepository;
            _studyGroupRepository = studyGroupRepository;
            _departmentRepository = departmentRepository;
            _logger = logger;
            _reportsPath = Path.Combine(_environment.ContentRootPath, "Reports");
            // Ensure Reports directory exists
            if (!Directory.Exists(_reportsPath))
            {
                Directory.CreateDirectory(_reportsPath);
            }
        }

        /// <summary>
        /// Generates a PDF report for student attendance
        /// </summary>
        public async Task<byte[]> GenerateStudentReportAsync(int studentId, DateTime? startDate, DateTime? endDate)
        {
            // Load template
            var templatePath = Path.Combine(_reportsPath, "StudentReport.frx");
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Report template not found at {templatePath}");
            }

            // Get student data
            var student = await _studentRepository.GetByIdAsync(studentId, CancellationToken.None);
            if (student?.User == null)
            {
                throw new NotFoundException($"Student or user details not found for ID {studentId}");
            }

            // Get attendance records
            var attendance = await _attendanceRepository.GetDetailedStudentAttendanceAsync(
        studentId, startDate, endDate, CancellationToken.None);

            // Create and populate data table
            var dataTable = new DataTable();
            dataTable.Columns.Add("Date", typeof(DateTime));
            dataTable.Columns.Add("StudyGroup", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("CheckInTime", typeof(DateTime));
            dataTable.Columns.Add("ConfirmationTime", typeof(DateTime));
            dataTable.Columns.Add("SessionTime", typeof(string));

            int totalPresent = 0;
            int totalAbsent = 0;

            // Add all records
            foreach (var record in attendance.Where(r => r?.CourseSession != null)
                                           .OrderByDescending(r => r.CourseSession.Date))
            {
                var status = record.IsConfirmed ? (record.IsAbsent ? "Absent" : "Present") : "Pending";
                if (record.IsConfirmed)
                {
                    if (record.IsAbsent)
                        totalAbsent++;
                    else
                        totalPresent++;
                }

                dataTable.Rows.Add(
                    record.CourseSession.Date,
                    record.CourseSession.StudyGroup?.Name ?? "Unknown Group",
                    status,
                    record.CheckInTime,
                    record.ConfirmationTime,
                    $"{record.CourseSession.StartTime.ToString(@"hh\:mm")}-{record.CourseSession.EndTime.ToString(@"hh\:mm")}"
                );
            }

            // Setup report
            using var report = new FastReport.Report();
            report.Load(templatePath);
            report.Dictionary.Clear();

            // Register data correctly
            report.RegisterData(dataTable, "AttendanceRecords");
            report.GetDataSource("AttendanceRecords").Enabled = true;

            // Set parameters
            report.SetParameterValue("StudentName", $"{student.User.FirstName} {student.User.LastName}".Trim());
            report.SetParameterValue("StudentId", student.StudentId ?? "N/A");
            report.SetParameterValue("DateRange", startDate.HasValue && endDate.HasValue
                ? $"{startDate.Value:d} - {endDate.Value:d}"
                : "All Dates");
            report.SetParameterValue("TotalRecords", dataTable.Rows.Count);
            report.SetParameterValue("TotalPresent", totalPresent);
            report.SetParameterValue("TotalAbsent", totalAbsent);
            report.SetParameterValue("AttendanceRate",
                totalPresent + totalAbsent == 0 ? 0 :
                Math.Round((decimal)totalPresent / (totalPresent + totalAbsent) * 100, 2));

            // Prepare report
            report.Prepare();

            // Export with optimized settings
            using var ms = new MemoryStream();
            var pdfExport = new PDFSimpleExport
            {
                ShowProgress = false,
                Subject = $"Attendance Report - {student.User.FirstName} {student.User.LastName}",
                JpegQuality = 100
            };

            pdfExport.Export(report, ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Generates a PDF report for group attendance
        /// </summary>
        public async Task<byte[]> GenerateGroupReportAsync(int groupId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                _logger.LogInformation("Starting group report generation for GroupID: {GroupId}", groupId);

                var templatePath = Path.Combine(_reportsPath, "GroupReport.frx");
                _logger.LogInformation("Template path: {TemplatePath}", templatePath);

                if (!File.Exists(templatePath))
                {
                    _logger.LogError("Template file not found at: {TemplatePath}", templatePath);
                    throw new FileNotFoundException($"Report template not found at {templatePath}");
                }

                // Get group data and validate
                var group = await _studyGroupRepository.GetByIdWithDetailsAsync(groupId, CancellationToken.None);
                if (group == null)
                {
                    _logger.LogError("Study group not found with ID: {GroupId}", groupId);
                    throw new NotFoundException($"Study group with ID {groupId} not found");
                }

                // Get attendance records and process data
                var studentIds = group.Students.Select(gs => gs.StudentId).ToList();
                _logger.LogInformation("Found {StudentCount} enrolled students", studentIds.Count);

                var allAttendance = new List<AttendanceRecord>();
                foreach (var studentId in studentIds)
                {
                    var records = await _attendanceRepository.GetDetailedStudentAttendanceAsync(
                        studentId, startDate, endDate, CancellationToken.None);
                    allAttendance.AddRange(records);
                }

                var filteredRecords = allAttendance
                    .Where(r => r?.CourseSession?.StudyGroupId == groupId)
                    .ToList();

                _logger.LogInformation("Processing {Count} attendance records", filteredRecords.Count);

                // Create data table with proper structure
                var dataTable = new DataTable("AttendanceRecords");
                dataTable.Columns.Add("Date", typeof(DateTime));
                dataTable.Columns.Add("StudentName", typeof(string));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("AttendanceRate", typeof(decimal));

                // Populate data
                var studentGroups = filteredRecords.GroupBy(r => r.StudentId);
                foreach (var studentGroup in studentGroups)
                {
                    var student = studentGroup.First().Student;
                    var presentCount = studentGroup.Count(r => r.IsConfirmed && !r.IsAbsent);
                    var totalCount = studentGroup.Count();
                    var attendanceRate = totalCount > 0 ? (decimal)presentCount * 100 / totalCount : 0;

                    foreach (var record in studentGroup)
                    {
                        dataTable.Rows.Add(
                            record.CourseSession.Date,
                            $"{student.User.FirstName} {student.User.LastName}",
                            record.IsConfirmed ? (record.IsAbsent ? "Absent" : "Present") : "Pending",
                            Math.Round(attendanceRate, 2)
                        );
                    }
                }

                _logger.LogInformation("Added {Count} rows to data table", dataTable.Rows.Count);

                // Setup report
                using var report = new FastReport.Report();
                report.Load(templatePath);
                report.Dictionary.Clear();

                // Register data source directly
                report.RegisterData(dataTable, "AttendanceRecords");
                report.GetDataSource("AttendanceRecords").Enabled = true;

                // Set parameters
                report.SetParameterValue("GroupName", group.Name);
                report.SetParameterValue("DateRange",
                    startDate.HasValue && endDate.HasValue
                        ? $"{startDate.Value:d} - {endDate.Value:d}"
                        : "All Time");
                report.SetParameterValue("TotalStudents", studentIds.Count);

                // Prepare and verify
                _logger.LogInformation("Preparing report with {Count} records", dataTable.Rows.Count);
                report.Prepare();

                // Export to PDF
                using var ms = new MemoryStream();
                var pdfExport = new PDFSimpleExport
                {
                    ShowProgress = false,
                    Subject = $"Group Attendance Report - {group.Name}",
                    JpegQuality = 100
                };

                pdfExport.Export(report, ms);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate group report: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Generates a PDF report for department attendance
        /// </summary>
        public async Task<byte[]> GenerateDepartmentReportAsync(int departmentId, int? academicYearId)
        {
            // Load template
            var templatePath = Path.Combine(_reportsPath, "DepartmentReport.frx");
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Report template not found at {templatePath}");
            }

            // Get department data
            var department = await _departmentRepository.GetByIdAsync(departmentId, CancellationToken.None);
            if (department == null)
            {
                throw new NotFoundException($"Department with ID {departmentId} not found");
            }

            // Get attendance stats
            var stats = await _attendanceRepository.GetDepartmentAttendanceStatsAsync(
                departmentId, academicYearId, CancellationToken.None);

            // Create and populate data table
            var dataTable = new DataTable("DepartmentStats");
            dataTable.Columns.Add("TotalStudents", typeof(int));
            dataTable.Columns.Add("AttendanceRate", typeof(decimal));
            dataTable.Columns.Add("PresentCount", typeof(int));
            dataTable.Columns.Add("AbsentCount", typeof(int));
            dataTable.Columns.Add("PendingCount", typeof(int));

            // Add stats row
            dataTable.Rows.Add(
                stats.TotalStudents,
                Math.Round(stats.AttendanceRate, 2),
                stats.PresentCount,
                stats.AbsentCount,
                stats.PendingCount
            );

            // Setup report
            using var report = new FastReport.Report();
            report.Load(templatePath);
            report.Dictionary.Clear();

            // Register data
            report.RegisterData(dataTable, "DepartmentStats");

            // Set parameters
            report.SetParameterValue("DepartmentName", department.Name);
            report.SetParameterValue("AcademicYear", academicYearId.HasValue
                ? $"Academic Year: {academicYearId}"
                : "All Academic Years");
            report.SetParameterValue("AverageAttendance", Math.Round(stats.AverageAttendance, 2));
            report.SetParameterValue("TotalSessions", stats.TotalSessions);

            // Prepare report
            report.Prepare();

            // Export with optimized settings
            using var ms = new MemoryStream();
            var pdfExport = new PDFSimpleExport
            {
                ShowProgress = false,
                Subject = $"Department Attendance Report - {department.Name}",
                JpegQuality = 100
            };

            pdfExport.Export(report, ms);
            return ms.ToArray();
        }
    }
}