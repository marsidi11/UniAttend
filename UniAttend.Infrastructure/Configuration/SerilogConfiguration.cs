using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace UniAttend.Infrastructure.Configuration
{
    /// <summary>
    /// Provides configuration for Serilog logging.
    /// </summary>
    public static class SerilogConfiguration
    {
        /// <summary>
        /// Configures logging for the host builder using Serilog.
        /// </summary>
        /// <param name="builder">The IHostBuilder to configure.</param>
        /// <returns>The configured IHostBuilder.</returns>
        public static IHostBuilder ConfigureLogging(this IHostBuilder builder)
        {
            return builder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithMachineName()
                    .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .WriteTo.File(
                        path: Path.Combine("logs", "general-.log"),
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .WriteTo.File(
                        path: Path.Combine("logs", "errors-.log"),
                        restrictedToMinimumLevel: LogEventLevel.Error,
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .WriteTo.File(
                        new JsonFormatter(),
                        Path.Combine("logs", "email-.json"),
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        rollingInterval: RollingInterval.Day,
                        shared: true
                    );
            });
        }
    }
}