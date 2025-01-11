using MediatR;
using System.Text.Json.Serialization;
using UniAttend.Application.Features.Subjects.DTOs;

namespace UniAttend.Application.Features.Subjects.Commands.CreateSubject
{
    public record CreateSubjectCommand : IRequest<SubjectDto>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int DepartmentId { get; init; }
        public int Credits { get; init; }
        [JsonIgnore]
        public bool IsActive { get; init; } = true;
    }
}