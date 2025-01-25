using MediatR;
using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Students.DTOs;

namespace UniAttend.Application.Features.Students.Queries.GetStudentAttendance
{
    public class GetStudentAttendanceQueryHandler 
        : IRequestHandler<GetStudentAttendanceQuery, IEnumerable<StudentAttendanceDto>>
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;

        public GetStudentAttendanceQueryHandler(IAttendanceRecordRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IEnumerable<StudentAttendanceDto>> Handle(
            GetStudentAttendanceQuery request, 
            CancellationToken cancellationToken)
        {
            // Get attendance records with included navigation properties
            var records = await _attendanceRepository.GetDetailedStudentAttendanceAsync(
                request.StudentId,
                request.StartDate,
                request.EndDate,
                cancellationToken);

            // Map to DTOs
            return records.Select(r => new StudentAttendanceDto
            {
                CourseSessionId = r.CourseSessionId,
                StudyGroupName = r.CourseSession?.StudyGroup?.Name ?? "Unknown Group",
                SubjectName = r.CourseSession?.StudyGroup?.Subject?.Name ?? "Unknown Subject",
                ClassroomName = r.CourseSession?.Classroom?.Name ?? "Unknown Classroom",
                CheckInTime = r.CheckInTime,
                CheckInMethod = r.CheckInMethod,
                IsConfirmed = r.IsConfirmed,
                ConfirmationTime = r.ConfirmationTime,
                StartTime = r.CourseSession?.StartTime ?? TimeSpan.Zero,
                EndTime = r.CourseSession?.EndTime ?? TimeSpan.Zero
            });
        }
    }
}