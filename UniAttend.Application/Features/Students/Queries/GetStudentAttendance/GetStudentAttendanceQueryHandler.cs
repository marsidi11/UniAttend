using MediatR;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Application.Features.Students.DTOs;

namespace UniAttend.Application.Features.Students.Queries.GetStudentAttendance
{
    public class GetStudentAttendanceQueryHandler 
        : IRequestHandler<GetStudentAttendanceQuery, IEnumerable<StudentAttendanceDto>>
    {
        private readonly IAttendanceRecordRepository _attendanceRepository;
        private readonly ICourseRepository _courseRepository;

        public GetStudentAttendanceQueryHandler(
            IAttendanceRecordRepository attendanceRepository,
            ICourseRepository courseRepository)
        {
            _attendanceRepository = attendanceRepository;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<StudentAttendanceDto>> Handle(
            GetStudentAttendanceQuery request, 
            CancellationToken cancellationToken)
        {
            var records = await _attendanceRepository.GetByStudentIdAsync(
                request.StudentId, 
                cancellationToken);

            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                records = records.Where(r => 
                    r.CheckInTime >= request.StartDate.Value && 
                    r.CheckInTime <= request.EndDate.Value);
            }

            return await Task.WhenAll(records.Select(async r =>
            {
                var course = await _courseRepository.GetByIdAsync(r.CourseId, cancellationToken);
                return new StudentAttendanceDto
                {
                    CourseId = r.CourseId,
                    CourseName = course?.Name ?? "Unknown",
                    CheckInTime = r.CheckInTime,
                    CheckInMethod = r.CheckInMethod,
                    IsConfirmed = r.IsConfirmed
                };
            }));
        }
    }
}