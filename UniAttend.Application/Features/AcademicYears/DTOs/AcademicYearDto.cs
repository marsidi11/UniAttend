using System;

namespace UniAttend.Application.Features.AcademicYears.DTOs
{
    public class AcademicYearDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public bool IsActive { get; init; }
        public int TotalGroups { get; init; }
        public int TotalStudents { get; init; }
    }
}