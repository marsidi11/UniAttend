using AutoMapper;
using MediatR;
using UniAttend.Application.Features.Subjects.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Subjects.Queries.GetDepartmentSubjects
{
    public class GetDepartmentSubjectsQueryHandler 
        : IRequestHandler<GetDepartmentSubjectsQuery, IEnumerable<SubjectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentSubjectsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubjectDto>> Handle(
            GetDepartmentSubjectsQuery request, 
            CancellationToken cancellationToken)
        {
            var subjects = await _unitOfWork.Subjects
                .GetByDepartmentIdAsync(request.DepartmentId, cancellationToken);

            if (request.IsActive.HasValue)
            {
                subjects = subjects.Where(s => s.IsActive == request.IsActive.Value);
            }

            return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }
    }
}