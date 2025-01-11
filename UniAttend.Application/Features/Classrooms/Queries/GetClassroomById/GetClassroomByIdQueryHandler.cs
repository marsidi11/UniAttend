using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Classrooms.DTOs;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.Classrooms.Queries.GetClassroomById
{
    public class GetClassroomByIdQueryHandler : IRequestHandler<GetClassroomByIdQuery, ClassroomDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClassroomByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ClassroomDto> Handle(GetClassroomByIdQuery request, CancellationToken cancellationToken)
        {
            var classroom = await _unitOfWork.Classrooms.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Classroom with ID {request.Id} not found");

            return _mapper.Map<ClassroomDto>(classroom);
        }
    }
}