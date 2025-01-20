using AutoMapper;
using MediatR;
using UniAttend.Application.Features.Subjects.DTOs;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Subjects.Queries.GetSubjects
{
    public class GetSubjectsQueryHandler
        : IRequestHandler<GetSubjectsQuery, IEnumerable<SubjectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSubjectsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubjectDto>> Handle(GetSubjectsQuery request, CancellationToken cancellationToken)
        {
            var subjects = await _unitOfWork.Subjects.GetAllAsync(
        isActive: request.IsActive,
        departmentId: request.DepartmentId,
        cancellationToken: cancellationToken);

            // Apply filters
            if (request.DepartmentId.HasValue)
            {
                subjects = subjects.Where(s => s.DepartmentId == request.DepartmentId.Value);
            }

            if (request.IsActive.HasValue)
            {
                subjects = subjects.Where(s => s.IsActive == request.IsActive.Value);
            }

            return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }
    }
}