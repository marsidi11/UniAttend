using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Classrooms.DTOs;

namespace UniAttend.Application.Features.Classrooms.Queries.GetClassrooms
{
    public class GetClassroomsQueryHandler : IRequestHandler<GetClassroomsQuery, IEnumerable<ClassroomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClassroomsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassroomDto>> Handle(GetClassroomsQuery request, CancellationToken cancellationToken)
        {
            var classrooms = await _unitOfWork.Classrooms.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ClassroomDto>>(classrooms);
        }
    }
}