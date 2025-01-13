using FluentValidation;
using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Role).NotEmpty().IsEnumName(typeof(UserRole));
            
            When(x => x.Role == UserRole.Student.ToString(), () => {
                RuleFor(x => x.StudentId).NotEmpty();
                RuleFor(x => x.DepartmentId).NotNull();
            });
        }
    }
}