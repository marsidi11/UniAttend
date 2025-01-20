namespace UniAttend.Application.Features.Classrooms.DTOs
{
    public class ClassroomDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? ReaderDeviceId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}