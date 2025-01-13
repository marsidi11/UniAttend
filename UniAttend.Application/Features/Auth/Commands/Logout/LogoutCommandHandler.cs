using MediatR;
using UniAttend.Shared.Exceptions;
using UniAttend.Core.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace UniAttend.Application.Features.Auth.Commands.Logout
{
    /// <summary>
    /// Handles the logic for logging out a user.
    /// </summary>
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

                public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                throw new NotFoundException("User not found");
            }
        
            user.UpdateRefreshToken(refreshToken: string.Empty, expiryTime: DateTime.UtcNow);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            return Unit.Value;
        }
    }
}