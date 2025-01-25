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
                $"{r.Student.User.FirstName} {r.Student.User.LastName}",
                r.CheckInTime,
                r.CheckInMethod,
                r.IsConfirmed,
                r.ConfirmationTime,
                r.ConfirmedByProfessorId,
                r.CourseSession.StudyGroup.Name,
                r.CourseSession.Classroom.Name,
                r.CourseSession.StartTime,
                r.CourseSession.EndTime
            ));
        }
    }
}