using MediatR;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Attendance.DTOs;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Attendance.Commands.SetupTotp
{
    public class SetupTotpCommandHandler : IRequestHandler<SetupTotpCommand, TotpSetupDto>
    {
        private readonly ITotpService _totpService;
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SetupTotpCommandHandler(
            ITotpService totpService,
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork)
        {
            _totpService = totpService;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TotpSetupDto> Handle(SetupTotpCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken)
                ?? throw new NotFoundException("Student not found");

            var secretKey = _totpService.GenerateSecretKey();
            var qrCodeUri = _totpService.GenerateQrCodeUri(secretKey, request.Email);

            student.User.SetupTwoFactor(secretKey);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TotpSetupDto(secretKey, qrCodeUri);
        }
    }
}