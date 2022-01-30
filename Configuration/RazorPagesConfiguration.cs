namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class RazorPagesConfiguration
{
    public static IMvcBuilder ConfigureRazorPages(this WebApplicationBuilder builder,
        IEnumerable<PolicyRole> policyRoles)
    {
        return builder.Services
            .AddRazorPages(options =>
            {
                options.Conventions.AddPageRoute("/WakeOnLan/Index", "/");
                foreach (var policyRole in policyRoles)
                {
                    options.Conventions.AuthorizeFolder(policyRole.Folder, policyRole.Policy);
                }
            })
            .AddSessionStateTempDataProvider();
    }
}