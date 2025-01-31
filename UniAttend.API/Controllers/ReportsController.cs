using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Reports.DTOs;
using UniAttend.Application.Features.Reports.Queries.GetAttendanceReport;
using UniAttend.Application.Features.Reports.Queries.GetDepartmentReport;
using UniAttend.Application.Features.Reports.Queries.GetGroupReport;
using UniAttend.Application.Features.Reports.Queries.GetStudentReport;
using UniAttend.API.Extensions;
using UniAttend.Application.Features.Reports.Queries.GetAcademicYearReport;
using UniAttend.Core.Interfaces.Services;

namespace UniAttend.API.Controllers
{
    /// <summary>
    /// Controller for handling report generation and retrieval
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReportService _reportService;

        /// <summary>
        /// Initializes a new instance of the ReportsController
        /// </summary>
        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a detailed attendance report for a specific student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="startDate">Optional start date for filtering</param>
        /// <param name="endDate">Optional end date for filtering</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Student attendance report</returns>
        [HttpGet("students/{id}")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<ActionResult<StudentReportDto>> GetStudentReport(
            int id,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            CancellationToken cancellationToken)
        {
            var query = new GetStudentReportQuery
            {
                StudentId = id,
                StartDate = startDate,
                EndDate = endDate
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets the current student's attendance report
        /// </summary>
        /// <param name="startDate">Optional start date for filtering</param>
        /// <param name="endDate">Optional end date for filtering</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Student's own attendance report</returns>
        [HttpGet("my-report")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<StudentReportDto>> GetMyReport(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            CancellationToken cancellationToken)
        {
            var currentUserId = User.GetUserId();
            var query = new GetStudentReportQuery
            {
                StudentId = currentUserId,
                StartDate = startDate,
                EndDate = endDate
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets a study group's attendance report
        /// </summary>
        /// <param name="id">Study group ID</param>
        /// <param name="startDate">Optional start date for filtering</param>
        /// <param name="endDate">Optional end date for filtering</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Group attendance report</returns>
        [HttpGet("groups/{id}")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<ActionResult<GroupReportDto>> GetGroupReport(
            int id,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            CancellationToken cancellationToken)
        {
            var query = new GetGroupReportQuery
            {
                StudyGroupId = id,
                StartDate = startDate,
                EndDate = endDate
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets a department's attendance statistics
        /// </summary>
        /// <param name="id">Department ID</param>
        /// <param name="academicYearId">Optional academic year for filtering</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Department attendance report</returns>
        [HttpGet("departments/{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<DepartmentReportDto>> GetDepartmentReport(
            int id,
            [FromQuery] int? academicYearId,
            CancellationToken cancellationToken)
        {
            var query = new GetDepartmentReportQuery
            {
                DepartmentId = id,
                AcademicYearId = academicYearId
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets a filtered attendance report
        /// </summary>
        /// <param name="startDate">Start date for the report</param>
        /// <param name="endDate">End date for the report</param>
        /// <param name="departmentId">Optional department filter</param>
        /// <param name="subjectId">Optional subject filter</param>
        /// <param name="studyGroupId">Optional study group filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Filtered attendance report</returns>
        [HttpGet("attendance")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<ActionResult<AttendanceReportRecordDto>> GetAttendanceReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? departmentId,
            [FromQuery] int? subjectId,
            [FromQuery] int? studyGroupId,
            CancellationToken cancellationToken)
        {
            var query = new GetAttendanceReportQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                DepartmentId = departmentId,
                SubjectId = subjectId,
                StudyGroupId = studyGroupId
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Gets an academic year's attendance statistics
        /// </summary>
        /// <param name="id">Academic year ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Academic year report</returns>
        [HttpGet("academic-years/{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<ActionResult<AcademicYearReportDto>> GetAcademicYearReport(
            int id,
            CancellationToken cancellationToken)
        {
            var query = new GetAcademicYearReportQuery { AcademicYearId = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("export/students/{id}")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<IActionResult> ExportStudentReport(
            int id,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var fileContent = await _reportService.GenerateStudentReportAsync(id, startDate, endDate);
            return File(
                fileContent, 
                "application/pdf",
                $"student-report-{id}-{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("export/groups/{id}")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<IActionResult> ExportGroupReport(
            int id,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var fileContent = await _reportService.GenerateGroupReportAsync(id, startDate, endDate);
            return File(
                fileContent,
                "application/pdf",
                $"group-report-{id}-{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("export/departments/{id}")]
        [Authorize(Roles = "Admin,Secretary")]
        public async Task<IActionResult> ExportDepartmentReport(
            int id,
            [FromQuery] int? academicYearId)
        {
            var fileContent = await _reportService.GenerateDepartmentReportAsync(id, academicYearId);
            return File(
                fileContent,
                "application/pdf",
                $"department-report-{id}-{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("export/attendance/{studyGroupId}")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<IActionResult> ExportAttendanceReport(
            int studyGroupId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var fileContent = await _reportService.GenerateAttendanceReportAsync(
                studyGroupId, startDate, endDate);
            return File(
                fileContent,
                "application/pdf",
                $"attendance-report-{studyGroupId}-{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}