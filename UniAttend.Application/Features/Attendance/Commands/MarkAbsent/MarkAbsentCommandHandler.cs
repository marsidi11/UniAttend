using MediatR;
using UniAttend.Core.Interfaces.Services;

namespace UniAttend.Application.Features.Attendance.Commands.MarkAbsent
{
    public class MarkAbsentCommandHandler : IRequestHandler<MarkAbsentCommand, Unit>
    {
        private readonly IAttendanceRecordService _attendanceService;

        public MarkAbsentCommandHandler(IAttendanceRecordService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public async Task<Unit> Handle(MarkAbsentCommand request, CancellationToken cancellationToken)
        {
            await _attendanceService.MarkStudentAbsentAsync(
                request.CourseSessionId,
                request.StudentId,
                cancellationToken);

            return Unit.Value;
        }
    }
}