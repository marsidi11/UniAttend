using AutoMapper;
using UniAttend.Application.Features.Students.DTOs;
using UniAttend.Core.Entities;

namespace UniAttend.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentListDto>();
            // Add other mappings as needed
        }
    }
}