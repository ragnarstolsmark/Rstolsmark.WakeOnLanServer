using Microsoft.AspNetCore.Mvc.Razor;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class RazorPagesConfiguration
{
    public static IMvcBuilder ConfigureRazorPages(this WebApplicationBuilder builder, IReadOnlyCollection<PolicyRole> policyRoles)
    {
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                "en-US",
                "nb-NO",
                "nn-NO"
            };
            options.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });
        return builder.Services
            .AddLocalization()
            .AddRazorPages(options =>
            {
                options.Conventions.AddPageRoute("/WakeOnLan/Index", "/");
                foreach (var policyRole in policyRoles)
                {
                    options.Conventions.AuthorizeFolder(policyRole.Folder, policyRole.Policy);
                }
            })
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddSessionStateTempDataProvider();
    }
}