using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Entities;
using UniAttend.Application.Features.AcademicYears.DTOs;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.AcademicYears.Commands.CreateAcademicYear
{
    public class CreateAcademicYearCommandHandler : IRequestHandler<CreateAcademicYearCommand, AcademicYearDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAcademicYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AcademicYearDto> Handle(CreateAcademicYearCommand request, CancellationToken cancellationToken)
        {
            if (request.IsActive)
            {
                var currentActive = await _unitOfWork.AcademicYears.GetCurrentAsync(cancellationToken);
                if (currentActive != null)
                {
                    throw new ValidationException("There can only be one active academic year at a time. Please deactivate the current active year first.");
                }
            }
        
            var academicYear = new AcademicYear(
                request.Name,
                request.StartDate,
                request.EndDate,
                request.IsActive
            );
        
            await _unitOfWork.AcademicYears.AddAsync(academicYear, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            return _mapper.Map<AcademicYearDto>(academicYear);
        }
    }
}