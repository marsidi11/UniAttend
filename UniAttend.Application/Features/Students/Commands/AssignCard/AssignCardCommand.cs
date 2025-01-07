using MediatR;

namespace UniAttend.Application.Features.Students.Commands.AssignCard
{
    public class AssignCardCommand : IRequest
    {
        public int StudentId { get; set; }
        public string CardId { get; set; } = string.Empty;
    }
}