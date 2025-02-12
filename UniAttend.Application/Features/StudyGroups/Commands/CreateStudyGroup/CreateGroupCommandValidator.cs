using FluentValidation;

namespace UniAttend.Application.Features.StudyGroups.Commands.CreateStudyGroup
{
    public class CreateStudyGroupCommandValidator : AbstractValidator<CreateStudyGroupCommand>
    {
        public CreateStudyGroupCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("StudyGroup name is required and cannot exceed 100 characters");

            RuleFor(x => x.SubjectId)
                .GreaterThan(0)
                .WithMessage("Valid subject ID is required");

            RuleFor(x => x.AcademicYearId)
                .GreaterThan(0)
                .WithMessage("Valid academic year ID is required");

            RuleFor(x => x.ProfessorId)
                .GreaterThan(0)
                .WithMessage("Valid professor ID is required");
        }
    }
}