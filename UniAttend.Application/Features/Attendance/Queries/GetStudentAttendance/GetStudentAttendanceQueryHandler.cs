using MediatR;
using UniAttend.Application.Common.Exceptions;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Queries.GetStudentAttendance
{
    public class GetStudentAttendanceQueryHandler : IRequestHandler<GetStudentAttendanceQuery, IEnumerable<AttendanceRecordDto>>
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;

        public GetStudentAttendanceQueryHandler(IAttendanceRecordRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IEnumerable<AttendanceRecordDto>> Handle(GetStudentAttendanceQuery request, CancellationToken cancellationToken)
        {
            var records = await _attendanceRepository.GetByStudentIdAsync(request.StudentId, cancellationToken);
            
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                records = records.Where(r => 
                    r.CheckInTime >= request.StartDate.Value && 
                    r.CheckInTime <= request.EndDate.Value);
            }

            return records.Select(r => new AttendanceRecordDto(
                r.CheckInTime,
                r.CheckInMethod,
                r.IsConfirmed,
                "Course Name", // You might want to fetch this from course repository
                "Professor Name" // You might want to fetch this from professor repository
            ));
        }
    }
}