using MediatR;
using UniAttend.Application.Features.Classes.DTOs;

namespace UniAttend.Application.Features.Classes.Commands.OpenClass
{
    public class OpenClassCommand : IRequest<ClassDto>
    {
        public int StudyGroupId { get; set; }
        public int ClassroomId { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}