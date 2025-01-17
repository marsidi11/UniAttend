using AutoMapper;
using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Students.DTOs;
using UniAttend.Application.Features.Students.Queries.GetStudentsList;

namespace UniAttend.Application.Features.Students.Queries.GetStudentsList
{
    public class GetStudentsListQueryHandler : IRequestHandler<GetStudentsListQuery, List<StudentListDto>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetStudentsListQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<List<StudentListDto>> Handle(GetStudentsListQuery request, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetAllWithDetailsAsync(cancellationToken);

            if (request.ActiveOnly)
            {
                students = students.Where(s => s.User?.IsActive == true);
            }

            if (request.DepartmentId.HasValue)
            {
                students = students.Where(s => s.DepartmentId == request.DepartmentId.Value);
            }

            var studentDtos = _mapper.Map<List<StudentListDto>>(students);
            return studentDtos;
        }
    }
}