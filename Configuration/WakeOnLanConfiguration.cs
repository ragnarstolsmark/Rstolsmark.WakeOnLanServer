using FluentValidation;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class WakeOnLanConfiguration
{
    public static void ConfigureWakeOnLan(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IValidator<Computer>, ComputerValidator>();
        builder.Services.AddScoped<ComputerService>();
    }
}