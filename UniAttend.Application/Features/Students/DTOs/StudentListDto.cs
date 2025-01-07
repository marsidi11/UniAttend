namespace UniAttend.Application.Features.Students.DTOs
{
    public class StudentListDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? CardId { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public bool IsActive { get; set; }
    }
}