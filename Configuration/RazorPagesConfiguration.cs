using FluentValidation;
using FluentValidation.AspNetCore;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class RazorPagesConfiguration
{
    public static IMvcBuilder ConfigureRazorPages(this WebApplicationBuilder builder,
        IEnumerable<PolicyRole> policyRoles)
    {
        builder.Services.AddSingleton<IValidator<Computer>, ComputerValidator>();
        return builder.Services
            .AddRazorPages(options =>
            {
                options.Conventions.AddPageRoute("/WakeOnLan/Index", "/");
                foreach (var policyRole in policyRoles)
                {
                    options.Conventions.AuthorizeFolder(policyRole.Folder, policyRole.Policy);
                }
            })
            .AddFluentValidation()
            .AddSessionStateTempDataProvider();
    }
}