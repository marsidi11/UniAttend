using MediatR;
using UniAttend.Shared.Exceptions;
using UniAttend.Application.Features.CourseSessions.DTOs;
using UniAttend.Core.Entities.Attendance;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Enums;

namespace UniAttend.Application.Features.CourseSessions.Commands.OpenCourseSession
{
    /// <summary>
    /// Handles the opening of a course session.
    /// </summary>
    public class OpenCourseSessionCommandHandler : IRequestHandler<OpenCourseSessionCommand, CourseSessionDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the OpenCourseSessionCommandHandler class.
        /// </summary>
        public OpenCourseSessionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processes the command to open a course session and returns the course session details.
        /// </summary>
        public async Task<CourseSessionDto> Handle(OpenCourseSessionCommand request, CancellationToken cancellationToken)
        {
            var studyGroup = await _unitOfWork.StudyGroups.GetByIdAsync(request.StudyGroupId, cancellationToken)
                ?? throw new NotFoundException($"StudyGroup with ID {request.StudyGroupId} not found");

            var classroom = await _unitOfWork.Classrooms.GetByIdAsync(request.ClassroomId, cancellationToken)
                ?? throw new NotFoundException($"Classroom with ID {request.ClassroomId} not found");

            var courseSession = new CourseSession(
                studyGroupId: request.StudyGroupId,
                classroomId: request.ClassroomId,
                date: request.Date,
                startTime: request.StartTime,
                endTime: request.EndTime,
                status: SessionStatus.Active
            );

            await _unitOfWork.CourseSessions.AddAsync(courseSession, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CourseSessionDto
            {
                Id = courseSession.Id,
                StudyGroupId = courseSession.StudyGroupId,
                StudyGroupName = studyGroup.Name,
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