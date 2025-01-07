using MediatR;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Students.Commands.RemoveCard
{
    public class RemoveCardCommandHandler : IRequestHandler<RemoveCardCommand>
    {
        private readonly IStudentRepository _studentRepository;

        public RemoveCardCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task Handle(RemoveCardCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId)
                ?? throw new NotFoundException(nameof(Student), request.StudentId);

            student.CardId = null;
            await _studentRepository.UpdateAsync(student);
        }
    }
}