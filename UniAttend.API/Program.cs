using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;
using Serilog.Events;
using UniAttend.API.Middleware;
using UniAttend.Application;
using UniAttend.Infrastructure;
using UniAttend.Infrastructure.Auth.Settings;
using UniAttend.Infrastructure.Data;
using UniAttend.Core.Interfaces.Services;
using UniAttend.Infrastructure.Settings;
using UniAttend.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog 
builder.Host.UseSerilog((context, services, configuration) => {
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .WriteTo.Console()
        .WriteTo.File(
            Path.Combine("logs", "general-.txt"),
            rollingInterval: RollingInterval.Day,
            shared: true)
        .WriteTo.File(
            Path.Combine("logs", "errors-.txt"),
            restrictedToMinimumLevel: LogEventLevel.Error,
            rollingInterval: RollingInterval.Day,
            shared: true);
});

// Configure JwtSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

// Add Distributed Cache
builder.Services.AddDistributedMemoryCache(); // For development environment

// Infrastructure & Application services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Add basic services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure FastReport
builder.Services.AddInfrastructure(builder.Configuration);

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
    if (jwtSettings == null) throw new InvalidOperationException("JWT settings are not configured");
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "http://192.168.0.101:5173" // Local netword IP
            )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniAttend API", Version = "v1" });
    
    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

// Email configuration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// Admin Configuration
builder.Services.Configure<AdminSettings>(
    builder.Configuration.GetSection("AdminSettings"));
builder.Services.AddScoped<AdminSetupService>();

// Build app
var app = builder.Build();

// Ensure Reports directory exists
var reportsPath = Path.Combine(app.Environment.ContentRootPath, "Reports");
if (!Directory.Exists(reportsPath))
{
    Directory.CreateDirectory(reportsPath);
}

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("VueApp");

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Error handling
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var adminSetup = scope.ServiceProvider.GetRequiredService<AdminSetupService>();
    await adminSetup.EnsureAdminCreatedAsync();
}

try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}