using FluentValidation;

namespace UniAttend.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100)
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password must be different from current password");
        }
    }
}