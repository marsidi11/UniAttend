using FluentValidation;

namespace UniAttend.Application.Features.Subjects.Commands.CreateSubject
{
    public class CreateSubjectCommandValidator : AbstractValidator<CreateSubjectCommand>
    {
        public CreateSubjectCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Subject name is required and cannot exceed 255 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .WithMessage("Valid department ID is required");

            RuleFor(x => x.Credits)
                .InclusiveBetween(1, 30)
                .WithMessage("Credits must be between 1 and 30");
        }
    }
}