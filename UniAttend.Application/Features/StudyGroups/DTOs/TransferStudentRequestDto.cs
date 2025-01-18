namespace UniAttend.Application.Features.StudyGroups.DTOs
{
    public class TransferStudentRequest
    {
        public int StudentId { get; init; }
        public int FromGroupId { get; init; }
        public int ToGroupId { get; init; }
    }
}