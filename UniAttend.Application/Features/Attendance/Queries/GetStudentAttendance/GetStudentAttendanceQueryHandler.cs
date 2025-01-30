using MediatR;
using UniAttend.Application.Common.Exceptions;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Attendance.DTOs;
using Microsoft.EntityFrameworkCore;

namespace UniAttend.Application.Features.Attendance.Queries.GetStudentAttendance
{
    public class GetStudentAttendanceQueryHandler 
        : IRequestHandler<GetStudentAttendanceQuery, IEnumerable<AttendanceRecordDto>>
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly ICourseSessionRepository _courseSessionRepository;
        private readonly IGroupStudentRepository _groupStudentRepository;

        public GetStudentAttendanceQueryHandler(
            IAttendanceRecordRepository attendanceRepository,
            ICourseSessionRepository courseSessionRepository,
            IGroupStudentRepository groupStudentRepository)
        {
            _attendanceRepository = attendanceRepository;
            _courseSessionRepository = courseSessionRepository;
            _groupStudentRepository = groupStudentRepository;
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
                r.Student?.User?.FirstName + " " + r.Student?.User?.LastName,
                r.CheckInTime,
                r.CheckInMethod,
                r.IsConfirmed,
                r.ConfirmationTime,
                r.CourseSession.StudyGroup.Name,
                r.CourseSession.Classroom.Name,
                r.CourseSession.StartTime,
                r.CourseSession.EndTime
            ));
        }
    }
}