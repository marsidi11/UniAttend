using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Enums;
using UniAttend.Application.Features.Users.DTOs;

namespace UniAttend.Application.Features.Users.Queries.GetUserDetails
{
    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

                        public async Task<UserDetailsDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
                        ?? throw new NotFoundException($"User with ID {request.UserId} not found");
                
                    var userDetails = _mapper.Map<UserDetailsDto>(user);
                    
                    // Initialize result with base user details
                    var result = userDetails;
                
                    // Get additional details based on role
                    switch (user.Role)
                    {
                        case UserRole.Student:
                            var student = await _unitOfWork.Students.GetByIdAsync(user.Id, cancellationToken);
                            if (student != null)
                            {
                                result = userDetails with
                                {
                                    DepartmentId = student.DepartmentId,
                                    DepartmentName = student.Department?.Name,
                                    Groups = await GetStudentGroups(student.Id, cancellationToken),
                                    AttendanceStats = await GetStudentAttendanceStats(student.Id, cancellationToken)
                                };
                            }
                            break;
                
                        case UserRole.Professor:
                            var professor = await _unitOfWork.Professors.GetByIdAsync(user.Id, cancellationToken);
                            if (professor != null)
                            {
                                result = userDetails with
                                {
                                    DepartmentId = professor.DepartmentId,
                                    DepartmentName = professor.Department?.Name,
                                    Groups = await GetProfessorStudyGroups(professor.Id, cancellationToken)
                                };
                            }
                            break;
                    }
                
                    return result;
                }

        private async Task<IEnumerable<UserGroupDto>> GetStudentGroups(int studentId, CancellationToken cancellationToken)
        {
            var studyGroups = await _unitOfWork.StudyGroups
                .GetStudentGroupsAsync(studentId, cancellationToken);

            return studyGroups.Select(g => new UserGroupDto
            {
                StudyGroupId = g.Id,
                StudyGroupName = g.Name,
                SubjectName = g.Subject.Name,
                AcademicYearName = g.AcademicYear.Name
            });
        }

        private async Task<AttendanceStatsDto> GetStudentAttendanceStats(int studentId, CancellationToken cancellationToken)
        {
            var stats = await _unitOfWork.AttendanceRecords
                .GetStudentStatsAsync(studentId, cancellationToken);

            return new AttendanceStatsDto
            {
                TotalCourseSessions = stats.TotalCourseSessions,
                AttendedcourseSessions = stats.AttendedcourseSessions,
                AttendanceRate = stats.AttendanceRate
            };
        }

        private async Task<IEnumerable<UserGroupDto>> GetProfessorStudyGroups(int professorId, CancellationToken cancellationToken)
        {
            var studyGroups = await _unitOfWork.StudyGroups
                .GetProfessorStudyGroupsAsync(professorId, cancellationToken);

            return studyGroups.Select(g => new UserGroupDto
            {
                StudyGroupId = g.Id,
                StudyGroupName = g.Name,
                SubjectName = g.Subject.Name,
                AcademicYearName = g.AcademicYear.Name
            });
        }
    }
}