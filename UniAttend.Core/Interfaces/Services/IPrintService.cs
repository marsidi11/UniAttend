namespace UniAttend.Core.Interfaces.Services
{
    public interface IPrintService
    {
        Task<byte[]> GenerateAttendanceReportPdfAsync(int studyGroupId, DateTime startDate, DateTime endDate);
        Task<byte[]> GenerateAttendanceSheetPdfAsync(int sessionId);
        Task PrintAttendanceReportAsync(byte[] pdfContent);
    }
}