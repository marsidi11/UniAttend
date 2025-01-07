using MediatR;

namespace UniAttend.Application.Features.Students.Commands.RemoveCard
{
    public class RemoveCardCommand : IRequest
    {
        public int StudentId { get; set; }
    }
}