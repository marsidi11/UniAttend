using MediatR;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Students.Commands.AssignCard
{
    public class AssignCardCommandHandler : IRequestHandler<AssignCardCommand>
    {
        private readonly IStudentRepository _studentRepository;

        public AssignCardCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task Handle(AssignCardCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId)
                ?? throw new NotFoundException(nameof(Student), request.StudentId);
        
            student.AssignCard(request.CardId);
            await _studentRepository.UpdateAsync(student);
        }
    }
}