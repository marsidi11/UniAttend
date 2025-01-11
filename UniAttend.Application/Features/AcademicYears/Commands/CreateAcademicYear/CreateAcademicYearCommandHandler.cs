using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Entities;
using UniAttend.Application.Features.AcademicYears.DTOs;

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
            var academicYear = new AcademicYear(
                request.Name,
                request.StartDate,
                request.EndDate
            );

            await _unitOfWork.AcademicYears.AddAsync(academicYear, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AcademicYearDto>(academicYear);
        }
    }
}