namespace UniAttend.Application.Features.Groups.DTOs
{
    public class EnrollStudentsRequest
    {
        public IEnumerable<int> StudentIds { get; init; } = Array.Empty<int>();
    }
}