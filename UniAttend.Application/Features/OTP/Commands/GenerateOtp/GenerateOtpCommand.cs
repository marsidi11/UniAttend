using MediatR;
using UniAttend.Application.Features.OTP.DTOs;

namespace UniAttend.Application.Features.OTP.Commands.GenerateOtp
{
    public record GenerateOtpCommand : IRequest<OtpDto>
    {
        public int ClassId { get; init; }
        public int StudentId { get; init; }
    }
}