using Serilog;
using Serilog.Events;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class LoggingConfiguration
{
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }

    public static void ConfigureLogger(this WebApplicationBuilder builder)
    {

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
    }

    public static void ConfigureRequestLogging(this WebApplication app)
    {
        var useRequestLogging = app.Configuration.GetValue<bool>("UseRequestLogging");
        if (useRequestLogging)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = ((diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString());
                    diagnosticContext.Set("Username", httpContext.User?.Identity?.Name ?? "anonymous");
                });
            });
        }
    }
}