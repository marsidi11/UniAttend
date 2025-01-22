using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using UniAttend.Application.Features.Reports.DTOs;
using UniAttend.Application.Features.Reports.Queries.GetStudentReport;
using UniAttend.Application.Features.Reports.Queries.GetGroupReport;
using UniAttend.Application.Features.Reports.Queries.GetDepartmentReport;
using UniAttend.Application.Features.Reports.Queries.GetAttendanceReport;
using UniAttend.Application.Features.Reports.Queries.GetAcademicYearReport;
using UniAttend.Application.Features.Reports.Queries.ExportAttendanceReport;
using UniAttend.API.Extensions;

namespace UniAttend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

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

        [HttpGet("attendance")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<ActionResult<AttendanceReportDto>> GetAttendanceReport(
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

        [HttpGet("export/attendance")]
        [Authorize(Roles = "Admin,Secretary,Professor")]
        public async Task<IActionResult> ExportAttendanceReport(
            [FromQuery] int studyGroupId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            CancellationToken cancellationToken)
        {
            var query = new ExportAttendanceReportQuery
            {
                StudyGroupId = studyGroupId,
                StartDate = startDate,
                EndDate = endDate
            };
            var fileContent = await _mediator.Send(query, cancellationToken);
            return File(
                fileContent,
                "application/pdf",
                $"attendance-report-{studyGroupId}-{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}