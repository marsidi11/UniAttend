using MediatR;
using UniAttend.Application.Features.Attendance.DTOs;

namespace UniAttend.Application.Features.Attendance.Commands.SetupTotp
{
    public class SetupTotpCommand : IRequest<TotpSetupDto>
    {
        public int StudentId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}