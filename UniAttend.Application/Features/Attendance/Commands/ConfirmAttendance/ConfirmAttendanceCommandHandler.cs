using MediatR;
using UniAttend.Application.Common.Exceptions;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Attendance.Commands.ConfirmAttendance
{
    public class ConfirmAttendanceCommandHandler : IRequestHandler<ConfirmAttendanceCommand, Unit>
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmAttendanceCommandHandler(IAttendanceRecordRepository attendanceRepository, IUnitOfWork unitOfWork)
        {
            _attendanceRepository = attendanceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ConfirmAttendanceCommand request, CancellationToken cancellationToken)
        {
            await _attendanceRepository.ConfirmAttendanceRecordsAsync(request.ClassId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}