using Rstolsmark.WakeOnLanServer.Services.WakeOnLan;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class WakeOnLanConfiguration
{
    public static void ConfigureWakeOnLan(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ComputerService>();
    }
}