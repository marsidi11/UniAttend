using MediatR;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Application.Features.AbsenceAlerts.DTOs;
using UniAttend.Shared.Exceptions;

namespace UniAttend.Application.Features.AbsenceAlerts.Commands.GenerateAbsenceAlert
{
    public class GenerateAbsenceAlertCommandHandler : IRequestHandler<GenerateAbsenceAlertCommand, AbsenceAlertDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public GenerateAbsenceAlertCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

                public async Task<AbsenceAlertDto> Handle(GenerateAbsenceAlertCommand request, CancellationToken cancellationToken)
        {
            // Check if alert already exists
            if (await _unitOfWork.AbsenceAlerts.HasActiveAlertAsync(
                request.StudentId,
                request.StudyGroupId,
                cancellationToken))
            {
                throw new ValidationException("Active absence alert already exists for this student and group");
            }
        
            var student = await _unitOfWork.Students.GetByIdWithDetailsAsync(request.StudentId, cancellationToken)
                ?? throw new NotFoundException($"Student with ID {request.StudentId} not found");
        
            var group = await _unitOfWork.StudyGroups.GetByIdWithDetailsAsync(request.StudyGroupId, cancellationToken)
                ?? throw new NotFoundException($"StudyGroup with ID {request.StudyGroupId} not found");
        
            if (student.User?.Email == null)
            {
                throw new ValidationException("Student email is required for sending alerts");
            }
        
            var alert = new AbsenceAlert(request.StudentId, request.StudyGroupId, request.AbsencePercentage);
            await _unitOfWork.AbsenceAlerts.AddAsync(alert, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            // Send email alert
            await _emailService.SendAbsenceAlertAsync(
                student.User.Email,
                $"{student.User.FirstName} {student.User.LastName}",
                group.Subject?.Name ?? "Unknown Subject",
                request.AbsencePercentage,
                cancellationToken);
        
            // Mark alert as sent
            alert.MarkAsSent();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            return new AbsenceAlertDto
            {
                StudentId = alert.StudentId,
                StudyGroupId = alert.StudyGroupId,
                AbsencePercentage = alert.AbsencePercentage,
                EmailSent = alert.EmailSent,
                CreatedAt = alert.CreatedAt
            };
        }
    }
}