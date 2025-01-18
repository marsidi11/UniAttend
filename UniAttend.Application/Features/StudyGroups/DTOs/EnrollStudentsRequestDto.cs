namespace UniAttend.Application.Features.StudyGroups.DTOs
{
    public class EnrollStudentsRequest
    {
        public IEnumerable<int> StudentIds { get; init; } = Array.Empty<int>();
    }
}