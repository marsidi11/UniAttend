using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Subjects.DTOs;

namespace UniAttend.Application.Features.Subjects.Queries.GetSubjectById
{
    public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSubjectByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubjectDto?> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var subject = await _unitOfWork.Subjects.GetByIdAsync(request.Id, cancellationToken);
            if (subject == null)
                return null;

            var subjectDto = _mapper.Map<SubjectDto>(subject);
            var department = await _unitOfWork.Departments.GetByIdAsync(subject.DepartmentId, cancellationToken);
            if (department != null)
            {
                return subjectDto with { DepartmentName = department.Name };
            }

            return subjectDto;
        }
    }
}