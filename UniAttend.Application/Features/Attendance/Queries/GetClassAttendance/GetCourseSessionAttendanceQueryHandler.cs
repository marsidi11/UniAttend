using MediatR;
using Microsoft.EntityFrameworkCore;
using UniAttend.Application.Common.Exceptions;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Queries.GetCourseSessionAttendance
{
    public class GetCourseSessionAttendanceQueryHandler : IRequestHandler<GetCourseSessionAttendanceQuery, IEnumerable<AttendanceRecordDto>>
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;

        public GetCourseSessionAttendanceQueryHandler(IAttendanceRecordRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IEnumerable<AttendanceRecordDto>> Handle(GetCourseSessionAttendanceQuery request, CancellationToken cancellationToken)
        {
            var records = await _attendanceRepository.GetDetailedByCourseSessionIdAsync(
                request.CourseSessionId, 
                cancellationToken);

            if (request.Date.HasValue)
            {
                records = records.Where(r => r.CheckInTime.Date == request.Date.Value.Date);
            }

            return records.Select(r => new AttendanceRecordDto(
                r.Id,
                r.CourseSessionId,
                r.StudentId,
                $"{r.Student?.User?.FirstName ?? "Unknown"} {r.Student?.User?.LastName ?? ""}".Trim(),
                r.CheckInTime,
                r.CheckInMethod,
                r.IsConfirmed,
                r.IsAbsent,
                r.ConfirmationTime,
                r.CourseSession?.StudyGroup?.Name ?? "Unknown",
                r.CourseSession?.Classroom?.Name ?? "Unknown",
                r.CourseSession?.StartTime ?? TimeSpan.Zero,
                r.CourseSession?.EndTime ?? TimeSpan.Zero
            ));
        }
    }
}