using MediatR;
using UniAttend.Application.Common.Exceptions;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Queries
{
    public class GetClassAttendanceQueryHandler : IRequestHandler<GetClassAttendanceQuery, IEnumerable<AttendanceRecordDto>>
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;

        public GetClassAttendanceQueryHandler(IAttendanceRecordRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IEnumerable<AttendanceRecordDto>> Handle(GetClassAttendanceQuery request, CancellationToken cancellationToken)
        {
            var records = await _attendanceRepository.GetByCourseIdAsync(request.ClassId, cancellationToken);

            if (request.Date.HasValue)
            {
                records = records.Where(r => r.CheckInTime.Date == request.Date.Value.Date);
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