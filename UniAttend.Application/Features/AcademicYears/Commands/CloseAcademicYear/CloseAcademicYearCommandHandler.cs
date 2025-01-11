using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.AcademicYears.Commands.CloseAcademicYear
{
    public class CloseAcademicYearCommandHandler : IRequestHandler<CloseAcademicYearCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CloseAcademicYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CloseAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Academic year not found");

            academicYear.SetActive(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}