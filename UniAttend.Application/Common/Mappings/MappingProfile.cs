using AutoMapper;
using UniAttend.Application.Features.Students.DTOs;
using UniAttend.Application.Features.Departments.DTOs;
using UniAttend.Application.Features.AcademicYears.DTOs;
using UniAttend.Application.Features.Subjects.DTOs;
using UniAttend.Application.Features.StudyGroups.DTOs;
using UniAttend.Application.Features.CourseSessions.DTOs;
using UniAttend.Application.Features.Attendance.DTOs;
using UniAttend.Application.Features.Users.DTOs;
using UniAttend.Application.Features.Classrooms.DTOs;
using UniAttend.Core.Entities;
using UniAttend.Core.Enums;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentListDto>()
    .ForMember(d => d.Username, opt => opt.MapFrom(s => s.User.Username))
    .ForMember(d => d.Email, opt => opt.MapFrom(s => s.User.Email))
    .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.User.FirstName))
    .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.User.LastName))
    .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.User.IsActive))
    .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department.Name));

            CreateMap<User, UserDto>()
            .ForMember(d => d.DepartmentId, opt => opt.MapFrom((src, _, _, context) =>
                src.Role == UserRole.Professor && src.Professor != null ? src.Professor.DepartmentId :
                src.Role == UserRole.Student && src.Student != null ? src.Student.DepartmentId :
                (int?)null))
            .ForMember(d => d.DepartmentName, opt => opt.MapFrom((src, _, _, context) =>
                src.Role == UserRole.Professor && src.Professor?.Department != null ? src.Professor.Department.Name :
                src.Role == UserRole.Student && src.Student?.Department != null ? src.Student.Department.Name :
                null))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

            CreateMap<Department, DepartmentDto>()
                .ForMember(d => d.SubjectsCount, opt => opt.MapFrom(src => src.Subjects.Count))
                .ForMember(d => d.StudentsCount, opt => opt.MapFrom(src => src.Students.Count))
                .ForMember(d => d.ProfessorsCount, opt => opt.MapFrom(src => src.Professors.Count))
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<AcademicYear, AcademicYearDto>()
                .ForMember(d => d.TotalGroups, opt => opt.MapFrom(src => src.StudyGroups.Count))
                .ForMember(d => d.TotalStudents, opt =>
                    opt.MapFrom(src => src.StudyGroups.Sum(g => g.Students.Count)));

            CreateMap<Subject, SubjectDto>()
    .ForMember(dest => dest.DepartmentName,
        opt => opt.MapFrom(src => src.Department.Name))
    .ForMember(dest => dest.GroupsCount,
        opt => opt.MapFrom(src => src.StudyGroups.Count))
    .ForMember(dest => dest.StudentsCount,
        opt => opt.MapFrom(src => src.StudyGroups.SelectMany(g => g.Students).Count()))
    .ForMember(dest => dest.AverageAttendance,
        opt => opt.MapFrom((src, dest, member, context) =>
        {
            var records = src.StudyGroups.SelectMany(g => g.AttendanceRecords);
            if (!records.Any())
                return 0m;
            return records.Average(a => a.IsConfirmed ? 100m : 0m);
        }));

            CreateMap<StudyGroup, StudyGroupDto>()
                .ForMember(d => d.StudentsCount, opt => opt.MapFrom(src => src.Students.Count))
                .ForMember(d => d.AcademicYearName, opt => opt.MapFrom(src => src.AcademicYear.Name))
                .ForMember(d => d.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(d => d.ProfessorName, opt =>
                    opt.MapFrom(src => $"{src.Professor.User.FirstName} {src.Professor.User.LastName}"));

            CreateMap<CourseSession, CourseSessionDto>()
                .ForMember(d => d.StudyGroupName, opt => opt.MapFrom(src => src.StudyGroup.Name))
                .ForMember(d => d.ClassroomName, opt => opt.MapFrom(src => src.Classroom.Name));

            CreateMap<AttendanceRecord, AttendanceRecordDto>()
            .ConstructUsing((src, ctx) => new AttendanceRecordDto(
                src.Id,
                src.CourseSessionId,
                src.StudentId,
                $"{src.Student.User.FirstName} {src.Student.User.LastName}",
                src.CheckInTime,
                src.CheckInMethod,
                src.IsConfirmed,
                src.ConfirmationTime,
                src.ConfirmedByProfessorId,
                src.CourseSession.StudyGroup.Name,
                src.CourseSession.Classroom.Name,
                src.CourseSession.StartTime,
                src.CourseSession.EndTime
            ));

            CreateMap<User, UserDetailsDto>()
            .ForMember(d => d.Groups, opt => opt.Ignore())
            .ForMember(d => d.AttendanceStats, opt => opt.Ignore())
            .ForMember(d => d.DepartmentId, opt => opt.Ignore())
            .ForMember(d => d.DepartmentName, opt => opt.Ignore());

            CreateMap<Classroom, ClassroomDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ReaderDeviceId, opt => opt.MapFrom(s => s.ReaderDeviceId));
        }
    }
}