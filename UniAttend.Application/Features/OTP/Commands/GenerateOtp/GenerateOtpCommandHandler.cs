using MediatR;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Application.Features.OTP.DTOs;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.OTP.Commands.GenerateOtp
{
    public class GenerateOtpCommandHandler : IRequestHandler<GenerateOtpCommand, OtpDto>
    {
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseSessionRepository _courseSessionRepository;

        public GenerateOtpCommandHandler(
            IOtpService otpService, 
            IEmailService emailService,
            IStudentRepository studentRepository,
            ICourseSessionRepository courseSessionRepository)
        {
            _otpService = otpService;
            _emailService = emailService;
            _studentRepository = studentRepository;
            _courseSessionRepository = courseSessionRepository;
        }

        public async Task<OtpDto> Handle(GenerateOtpCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken)
                ?? throw new NotFoundException($"Student with ID {request.StudentId} not found");

            var courseSession = await _courseSessionRepository.GetByIdAsync(request.ClassId, cancellationToken)
                ?? throw new NotFoundException($"Class session with ID {request.ClassId} not found");

            var otpCode = await _otpService.GenerateOtpAsync(
                request.ClassId,
                request.StudentId,
                cancellationToken);

            // Send OTP code via email
            await _emailService.SendOtpCodeAsync(
                student.User.Email,
                otpCode.Code,
                courseSession.Course.Name,
                otpCode.ExpiryTime,
                cancellationToken);

            return new OtpDto
            {
                Code = otpCode.Code,
                ExpiryTime = otpCode.ExpiryTime,
                IsUsed = otpCode.IsUsed
            };
        }
    }
}