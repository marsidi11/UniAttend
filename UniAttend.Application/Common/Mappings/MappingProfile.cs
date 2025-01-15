using AutoMapper;
using UniAttend.Application.Features.Students.DTOs;
using UniAttend.Application.Features.Departments.DTOs;
using UniAttend.Application.Features.AcademicYears.DTOs;
using UniAttend.Application.Features.Subjects.DTOs;
using UniAttend.Application.Features.Groups.DTOs;
using UniAttend.Application.Features.Classes.DTOs;
using UniAttend.Application.Features.Attendance.DTOs;
using UniAttend.Application.Features.Users.DTOs;
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
            CreateMap<Student, StudentListDto>();

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
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(d => d.GroupsCount, opt => opt.MapFrom(src => src.StudyGroups.Count))
                .ForMember(d => d.StudentsCount, opt =>
                    opt.MapFrom(src => src.StudyGroups.Sum(g => g.Students.Count)))
                .ForMember(d => d.AverageAttendance, opt =>
                    opt.MapFrom(src => src.StudyGroups.SelectMany(g => g.AttendanceRecords)
                        .Average(a => a.IsConfirmed ? 1 : 0) * 100));

            CreateMap<StudyGroup, StudyGroupDto>()
                .ForMember(d => d.StudentsCount, opt => opt.MapFrom(src => src.Students.Count))
                .ForMember(d => d.AcademicYearName, opt => opt.MapFrom(src => src.AcademicYear.Name))
                .ForMember(d => d.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(d => d.ProfessorName, opt =>
                    opt.MapFrom(src => $"{src.Professor.User.FirstName} {src.Professor.User.LastName}"));

            CreateMap<CourseSession, ClassDto>()
                .ForMember(d => d.GroupName, opt => opt.MapFrom(src => src.Group.Name))
                .ForMember(d => d.ClassroomName, opt => opt.MapFrom(src => src.Classroom.Name));

            CreateMap<AttendanceRecord, AttendanceRecordDto>()
                .ConstructUsing((src, ctx) => new AttendanceRecordDto(
                    src.CheckInTime,
                    src.CheckInMethod,
                    src.IsConfirmed,
                    src.Course.Name,
                    $"{src.Course.Professor.User.FirstName} {src.Course.Professor.User.LastName}"
                ));

            CreateMap<User, UserDetailsDto>()
            .ForMember(d => d.Groups, opt => opt.Ignore())
            .ForMember(d => d.AttendanceStats, opt => opt.Ignore())
            .ForMember(d => d.DepartmentId, opt => opt.Ignore())
            .ForMember(d => d.DepartmentName, opt => opt.Ignore());
        }
    }
}