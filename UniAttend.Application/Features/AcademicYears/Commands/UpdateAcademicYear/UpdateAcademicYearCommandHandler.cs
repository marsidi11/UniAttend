using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Entities;

namespace UniAttend.Application.Features.AcademicYears.Commands.UpdateAcademicYear
{
    public class UpdateAcademicYearCommandHandler : IRequestHandler<UpdateAcademicYearCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAcademicYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Academic year not found");

            if (request.StartDate.HasValue || request.EndDate.HasValue)
            {
                var overlapping = await _unitOfWork.AcademicYears.HasOverlappingDatesAsync(
                    request.StartDate ?? academicYear.StartDate,
                    request.EndDate ?? academicYear.EndDate,
                    request.Id,
                    cancellationToken);

                if (overlapping)
                    throw new ValidationException("The dates overlap with another academic year");
            }

            if (request.Name != null)
                academicYear = new AcademicYear(
                    request.Name,
                    request.StartDate ?? academicYear.StartDate,
                    request.EndDate ?? academicYear.EndDate);

            if (request.IsActive.HasValue)
                academicYear.SetActive(request.IsActive.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}