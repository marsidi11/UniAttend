using MediatR;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Users.Commands.VerifyTotp
{
    public class VerifyTotpCommandHandler : IRequestHandler<VerifyTotpCommand, bool>
    {
        private readonly ITotpService _totpService;
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VerifyTotpCommandHandler(
            ITotpService totpService,
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork)
        {
            _totpService = totpService;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(VerifyTotpCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdWithUserAsync(request.StudentId, cancellationToken)
                ?? throw new NotFoundException("Student not found");

            if (string.IsNullOrEmpty(student.User.TotpSecret))
                throw new ValidationException("TOTP not set up for this user");

            var isValid = _totpService.VerifyTotp(student.User.TotpSecret, request.Code);
            
            if (isValid)
            {
                student.User.VerifyTwoFactor();
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return isValid;
        }
    }
}