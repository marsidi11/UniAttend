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
            // Get students with included navigation properties
            var groupStudents = await _groupStudentRepository
                .GetByGroupIdWithDetailsAsync(request.StudyGroupId, cancellationToken);
        
            return groupStudents.Select(gs => new GroupStudentDto
            {
                StudentId = gs.StudentId,
                StudentName = gs.Student?.User != null 
                    ? $"{gs.Student.User.FirstName ?? "N/A"} {gs.Student.User.LastName ?? ""}".Trim()
                    : "Unknown Student",
                StudentNumber = gs.Student?.StudentId ?? "N/A",
                AttendanceRate = 0, // Consider calculating this
                IsActive = gs.Student?.User?.IsActive ?? false
            });
        }
    }
}