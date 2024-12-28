using FluentValidation;

namespace UniAttend.Application.Auth.Commands.Logout
{
    /// <summary>
    /// Validates the LogoutCommand.
    /// </summary>
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be a positive integer.");
        }
    }
}