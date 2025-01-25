using AutoMapper;
using MediatR;
using UniAttend.Application.Features.AcademicYears.DTOs;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.AcademicYears.Queries.GetActiveAcademicYear
{
    public class GetActiveAcademicYearQueryHandler : IRequestHandler<GetActiveAcademicYearQuery, AcademicYearDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetActiveAcademicYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AcademicYearDto?> Handle(GetActiveAcademicYearQuery request, CancellationToken cancellationToken)
        {
            var activeYear = await _unitOfWork.AcademicYears.GetCurrentAsync(cancellationToken);
            return activeYear != null ? _mapper.Map<AcademicYearDto>(activeYear) : null;
        }
    }
}