using FluentValidation;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class WakeOnLanConfiguration
{
    public static void ConfigureWakeOnLan(this WebApplicationBuilder builder, List<PolicyRole> policyRoles)
    {
        builder.Services.AddSingleton<IValidator<Computer>, ComputerValidator>();
        builder.Services.AddScoped<ComputerService>();
        var wakeOnLanRole = builder.Configuration.GetValue<string>("WakeOnLanRole");
        var wakeOnLanAccessRequiresRole = !string.IsNullOrEmpty(wakeOnLanRole);
        if (wakeOnLanAccessRequiresRole)
        {
            policyRoles.Add( new PolicyRole(
                policy: "RequireWakeOnLanRole",
                role: wakeOnLanRole,
                folder: "/WakeOnLan")
            );
        }
    }
}