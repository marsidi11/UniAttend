using MediatR;

namespace UniAttend.Application.Features.Classes.Commands.CloseClass
{
    public record CloseClassCommand : IRequest<Unit>
    {
        public int ClassId { get; init; }
    }
}