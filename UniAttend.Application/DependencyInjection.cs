using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

/// <summary>
/// Provides extension methods for dependency injection configuration in the UniAttend application.
/// </summary>
/// <remarks>
/// This static class contains extension methods for registering application-specific services
/// and configurations with the dependency injection container.
/// </remarks>
namespace UniAttend.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add MediatR
            services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}