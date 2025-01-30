using MediatR;
using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Queries.GetStudentAttendance
{
    public class GetStudentAttendanceQueryHandler 
        : IRequestHandler<GetStudentAttendanceQuery, IEnumerable<AttendanceRecordDto>>
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;

        public GetStudentAttendanceQueryHandler(IAttendanceRecordRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IEnumerable<AttendanceRecordDto>> Handle(
            GetStudentAttendanceQuery request, 
            CancellationToken cancellationToken)
        {
            var records = await _attendanceRepository
                .GetDetailedStudentAttendanceAsync(
                    request.StudentId,
                    request.StartDate,
                    request.EndDate,
                    cancellationToken);

            return records.Select(r => new AttendanceRecordDto(
                r.Id,
                r.CourseSessionId,
                r.StudentId,
                $"{r.Student?.User?.FirstName} {r.Student?.User?.LastName}",
                r.CheckInTime,
                r.CheckInMethod,
                r.IsConfirmed,
                r.IsAbsent,
                r.ConfirmationTime,
                r.CourseSession.StudyGroup.Name,
                r.CourseSession.Classroom.Name,
                r.CourseSession.StartTime,
                r.CourseSession.EndTime
            ));
        }
    }
}