using AutoMapper;
using MediatR;
using UniAttend.Application.Features.AcademicYears.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.AcademicYears.Queries.GetAcademicYears
{
    public class GetAcademicYearsQueryHandler : IRequestHandler<GetAcademicYearsQuery, IEnumerable<AcademicYearDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAcademicYearsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AcademicYearDto>> Handle(GetAcademicYearsQuery request, CancellationToken cancellationToken)
        {
            var years = await _unitOfWork.AcademicYears.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AcademicYearDto>>(years);
        }
    }
}