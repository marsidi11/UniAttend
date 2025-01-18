using MediatR;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;
using UniAttend.Application.Features.StudyGroups.DTOs;

namespace UniAttend.Application.Features.StudyGroups.Commands.CreateGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, StudyGroupDto>
    {
        private readonly IStudyGroupRepository _groupRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGroupCommandHandler(
            IStudyGroupRepository groupRepository,
            ISubjectRepository subjectRepository,
            IProfessorRepository professorRepository,
            IAcademicYearRepository academicYearRepository,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _subjectRepository = subjectRepository;
            _professorRepository = professorRepository;
            _academicYearRepository = academicYearRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudyGroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            // Validate entities exist
            var subject = await _subjectRepository.GetByIdAsync(request.SubjectId, cancellationToken)
                ?? throw new NotFoundException($"Subject with ID {request.SubjectId} not found");

            var professor = await _professorRepository.GetByIdAsync(request.ProfessorId, cancellationToken)
                ?? throw new NotFoundException($"Professor with ID {request.ProfessorId} not found");

            var academicYear = await _academicYearRepository.GetByIdAsync(request.AcademicYearId, cancellationToken)
                ?? throw new NotFoundException($"Academic year with ID {request.AcademicYearId} not found");

            // Check if group name already exists
            if (await _groupRepository.ExistsWithNameAsync(
                request.Name,
                request.SubjectId,
                request.AcademicYearId,
                cancellationToken))
            {
                throw new ValidationException(
                    "A group with this name already exists for the same subject and academic year");
            }

            // Create new group
            var group = new StudyGroup(
                request.Name,
                request.SubjectId,
                request.AcademicYearId,
                request.ProfessorId);

            await _groupRepository.AddAsync(group, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return populated DTO
            return new StudyGroupDto
            {
                Id = group.Id,
                Name = group.Name,
                SubjectId = group.SubjectId,
                SubjectName = subject.Name,
                AcademicYearId = group.AcademicYearId,
                AcademicYearName = academicYear.Name,
                ProfessorId = group.ProfessorId,
                ProfessorName = $"{professor.User?.FirstName} {professor.User?.LastName}".Trim(),
                StudentsCount = 0,
                AttendanceRate = 0,
                IsActive = group.IsActive
            };
        }
    }
}