using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Entities;

namespace UniAttend.Application.Features.AcademicYears.Commands.CreateAcademicYear
{
    public class CreateAcademicYearCommandHandler : IRequestHandler<CreateAcademicYearCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAcademicYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var academicYear = new AcademicYear(
                request.Name,
                request.StartDate,
                request.EndDate
            );

            await _unitOfWork.AcademicYears.AddAsync(academicYear, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return academicYear.Id;
        }
    }
}