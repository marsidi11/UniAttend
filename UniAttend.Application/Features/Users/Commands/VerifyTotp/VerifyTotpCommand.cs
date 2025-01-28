using MediatR;

namespace UniAttend.Application.Features.Users.Commands.VerifyTotp
{
    public class VerifyTotpCommand : IRequest<bool>
    {
        public int StudentId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}