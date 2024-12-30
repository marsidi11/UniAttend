using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Entities.Attendance;

namespace UniAttend.Infrastructure.Services
{
    public class PdfPrintService : IPrintService
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly ILogger<PdfPrintService> _logger;

        public PdfPrintService(IAttendanceRecordRepository attendanceRepository,
                             ILogger<PdfPrintService> logger)
        {
            _attendanceRepository = attendanceRepository;
            _logger = logger;
        }

        public async Task<byte[]> GenerateAttendanceReportPdfAsync(int groupId,
        DateTime startDate, DateTime endDate)
        {
            try
            {
                var records = await _attendanceRepository.GetGroupAttendanceAsync(groupId, startDate, endDate);
                using var stream = new MemoryStream();
                using var writer = new PdfWriter(stream);
                using var pdf = new PdfDocument(writer);
                using var document = new Document(pdf);

                // Add header
                document.Add(new Paragraph($"Attendance Report")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));

                document.Add(new Paragraph($"Period: {startDate:d} - {endDate:d}")
                    .SetTextAlignment(TextAlignment.CENTER));

                // Create table
                var table = new Table(5).UseAllAvailableWidth();
                table.AddHeaderCell("Date");
                table.AddHeaderCell("Student");
                table.AddHeaderCell("Subject");
                table.AddHeaderCell("Status");
                table.AddHeaderCell("Time");

                foreach (var record in records)
                {
                    table.AddCell(record.CheckInTime.ToString("d"));
                    table.AddCell($"{record.Student.User.FirstName} {record.Student.User.LastName}");
                    table.AddCell(record.Course.StudyGroup.Subject.Name);
                    table.AddCell(record.IsConfirmed ? "Present" : "Pending");
                    table.AddCell(record.CheckInTime.ToString("t"));
                }

                document.Add(table);
                document.Close();

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating attendance report PDF for group {GroupId}", groupId);
                throw;
            }
        }

        public async Task<byte[]> GenerateAttendanceSheetPdfAsync(int sessionId)
        {
            try
            {
                var session = await _attendanceRepository.GetSessionWithDetailsAsync(sessionId);
                using var stream = new MemoryStream();
                using var writer = new PdfWriter(stream);
                using var pdf = new PdfDocument(writer);
                using var document = new Document(pdf);

                // Add header
                document.Add(new Paragraph($"Attendance Sheet")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));

                document.Add(new Paragraph($"Class: {session.Course.Name}")
                    .SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph($"Date: {session.Date:d}")
                    .SetTextAlignment(TextAlignment.CENTER));

                // Create sign-in table
                var table = new Table(4).UseAllAvailableWidth();
                table.AddHeaderCell("Student ID");
                table.AddHeaderCell("Name");
                table.AddHeaderCell("Time");
                table.AddHeaderCell("Signature");

                foreach (var groupStudent in session.Group.Students)
                {
                    table.AddCell(new Cell().Add(new Paragraph(groupStudent.Student.StudentId)));
                    table.AddCell(new Cell().Add(new Paragraph(
                        $"{groupStudent.Student.User.FirstName} {groupStudent.Student.User.LastName}")));
                    table.AddCell(new Cell().Add(new Paragraph(""))); // Empty cell for time
                    table.AddCell(new Cell().Add(new Paragraph(""))); // Empty cell for signature
                }

                document.Add(table);
                document.Close();

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating attendance sheet PDF for session {SessionId}", sessionId);
                throw;
            }
        }

        public async Task PrintAttendanceReportAsync(byte[] pdfContent)
        {
            try
            {
                // Save to temp file
                var tempFile = Path.GetTempFileName();
                await File.WriteAllBytesAsync(tempFile, pdfContent);

                // Open with default PDF viewer
                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempFile,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error printing attendance report");
                throw;
            }
        }
    }
}