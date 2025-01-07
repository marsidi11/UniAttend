using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniAttend.Application;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Infrastructure.Auth.Services;
using UniAttend.Infrastructure.Auth.Settings;
using UniAttend.Infrastructure.Data;
using UniAttend.Infrastructure.Data.Repositories;
using UniAttend.Infrastructure.Services;
using UniAttend.Infrastructure.Settings;

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
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // JWT Settings
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
                ?? throw new InvalidOperationException("JWT settings are not configured");
            services.AddSingleton(jwtSettings);

            services.AddHttpContextAccessor();

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Core Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IProfessorRepository, ProfessorRepository>();
            services.AddScoped<IAttendanceRecordRepository, AttendanceRecordRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IStudyGroupRepository, StudyGroupRepository>();
            services.AddScoped<IGroupStudentRepository, GroupStudentRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IClassroomRepository, ClassroomRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IOtpCodeRepository, OtpCodeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IAbsenceAlertRepository, AbsenceAlertRepository>();

            // Core Services
            services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<ICardReaderService, CardReaderService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IPrintService, PdfPrintService>();
            services.AddScoped<INetworkValidationService, NetworkValidationService>();

            // Configure Settings
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<CardReaderSettings>(configuration.GetSection("CardReaderSettings"));
            services.Configure<NetworkSettings>(configuration.GetSection("NetworkSettings"));

            // Add Application Layer Services
            services.AddApplication();

            return services;
        }
    }
}