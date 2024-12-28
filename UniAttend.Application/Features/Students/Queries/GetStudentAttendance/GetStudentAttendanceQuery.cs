using MediatR;
using System;
using UniAttend.Application.Features.Students.DTOs;

namespace UniAttend.Application.Features.Students.Queries.GetStudentAttendance
{
    public record GetStudentAttendanceQuery : IRequest<IEnumerable<StudentAttendanceDto>>
    {
        public int StudentId { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }
}