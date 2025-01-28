using MediatR;
using UniAttend.Application.Features.Users.DTOs;

namespace UniAttend.Application.Features.Users.Commands.SetupTotp
{
    public class SetupTotpCommand : IRequest<TotpSetupDto>
    {
        public int StudentId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}