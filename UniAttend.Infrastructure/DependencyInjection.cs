using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using UniAttend.Application;
using UniAttend.Application.Auth.Common;
using UniAttend.Application.Common.Interfaces;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Infrastructure.Data;
using UniAttend.Infrastructure.Data.Repositories;
using UniAttend.Infrastructure.Services;
using UniAttend.Infrastructure.Settings;

/// <summary>
/// Provides extension methods for configuring infrastructure services in the dependency injection container.
/// </summary>
/// <remarks>
/// This class configures core infrastructure services including:
/// - JWT authentication settings
/// - Authentication services
/// - Password hashing
/// - User repository
/// - Application layer services
/// </remarks>
namespace UniAttend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Configure JWT Settings
            var jwtSettings = new JwtSettings
            {
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                TokenExpirationInMinutes = 60
            };
            services.AddSingleton(jwtSettings);

            // Register Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IAttendanceRecordRepository, AttendanceRecordRepository>();
            services.AddScoped<IGroupStudentRepository, GroupStudentRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();

            // Register Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<ICardReaderService, CardReaderService>();
            services.AddScoped<IAuditService, AuditService>();

            // Add Repositories
            services.AddScoped<IClassroomRepository, ClassroomRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            // Configure email settings
            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings"));

            // Add Application Layer Services
            services.AddApplication();

            return services;
        }
    }
}