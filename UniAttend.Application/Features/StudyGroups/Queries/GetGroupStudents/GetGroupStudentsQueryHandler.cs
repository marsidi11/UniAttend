using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Queries.GetGroupStudents
{
    public class GetGroupStudentsQueryHandler 
        : IRequestHandler<GetGroupStudentsQuery, IEnumerable<GroupStudentDto>>
    {
        private readonly IGroupStudentRepository _groupStudentRepository;

        public GetGroupStudentsQueryHandler(IGroupStudentRepository groupStudentRepository)
        {
            _groupStudentRepository = groupStudentRepository;
        }

        public async Task<IEnumerable<GroupStudentDto>> Handle(
            GetGroupStudentsQuery request, 
            CancellationToken cancellationToken)
        {
            var groupStudents = await _groupStudentRepository
                .GetByGroupIdAsync(request.GroupId, cancellationToken);

            return groupStudents.Select(gs => new GroupStudentDto
            {
                StudentId = gs.StudentId,
                StudentName = $"{gs.Student?.User.FirstName} {gs.Student?.User.LastName}",
                StudentNumber = gs.Student?.StudentId ?? string.Empty,
                AttendanceRate = 0, // Calculate this based on attendance records
                IsActive = gs.Student?.User.IsActive ?? false
            });
        }
    }
}