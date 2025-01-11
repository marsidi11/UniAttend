using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Classrooms.DTOs;

namespace UniAttend.Application.Features.Classrooms.Queries.GetAvailableClassrooms
{
    public class GetAvailableClassroomsQueryHandler 
        : IRequestHandler<GetAvailableClassroomsQuery, IEnumerable<ClassroomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAvailableClassroomsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassroomDto>> Handle(
            GetAvailableClassroomsQuery request, 
            CancellationToken cancellationToken)
        {
            var classrooms = await _unitOfWork.Classrooms.GetAvailableAsync(
                request.StartTime, 
                request.EndTime, 
                cancellationToken);

            return _mapper.Map<IEnumerable<ClassroomDto>>(classrooms);
        }
    }
}