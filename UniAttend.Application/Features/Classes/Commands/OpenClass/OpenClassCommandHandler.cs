using MediatR;
using UniAttend.Shared.Exceptions;
using UniAttend.Application.Features.Classes.DTOs;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Application.Features.Classes.Commands.OpenClass
{
    public class OpenClassCommandHandler : IRequestHandler<OpenClassCommand, ClassDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public OpenClassCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClassDto> Handle(OpenClassCommand request, CancellationToken cancellationToken)
        {
            var group = await _unitOfWork.StudyGroups.GetByIdAsync(request.StudyGroupId, cancellationToken)
                ?? throw new NotFoundException($"StudyGroup with ID {request.StudyGroupId} not found");

            var classroom = await _unitOfWork.Classrooms.GetByIdAsync(request.ClassroomId, cancellationToken)
                ?? throw new NotFoundException($"Classroom with ID {request.ClassroomId} not found");

            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId, cancellationToken)
                ?? throw new NotFoundException($"Course with ID {request.CourseId} not found");

            var courseSession = new CourseSession(
                studyGroupId: request.StudyGroupId,
                classroomId: request.ClassroomId,
                courseId: request.CourseId,
                date: request.Date,
                startTime: request.StartTime,
                endTime: request.EndTime,
                status: "Active"
            );

            await _unitOfWork.CourseSessions.AddAsync(courseSession, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ClassDto
            {
                Id = courseSession.Id,
                StudyGroupId = courseSession.StudyGroupId,
                GroupName = group.Name,
                ClassroomId = courseSession.ClassroomId,
                ClassroomName = classroom.Name,
                Date = courseSession.Date,
                StartTime = courseSession.StartTime,
                EndTime = courseSession.EndTime,
                Status = courseSession.Status
            };
        }
    }
}