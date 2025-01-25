using Microsoft.Extensions.Options;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Enums;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Infrastructure.Settings;

namespace UniAttend.Infrastructure.Services
{
    public class AdminSetupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AdminSettings _adminSettings;

        public AdminSetupService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IOptions<AdminSettings> adminSettings)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _adminSettings = adminSettings.Value;
        }

        public async Task EnsureAdminCreatedAsync()
        {
            if (await _unitOfWork.Users.AnyAsync(u => u.Role == UserRole.Admin))
                return;

            var adminUser = new User(
                username: _adminSettings.Username,
                passwordHash: _passwordHasher.HashPassword(_adminSettings.Password),
                email: _adminSettings.Email,
                role: UserRole.Admin,
                firstName: _adminSettings.FirstName,
                lastName: _adminSettings.LastName
            );

            await _unitOfWork.Users.AddAsync(adminUser);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}