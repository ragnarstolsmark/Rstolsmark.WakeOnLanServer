using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class AzureAdConfiguration
{
    public static void AddAzureAdAuthentication(this WebApplicationBuilder builder, IMvcBuilder mvcBuilder,
        IConfigurationSection azureAdConfiguration, IEnumerable<PolicyRole> policyRoles)
    {
        mvcBuilder.AddMicrosoftIdentityUI();
        builder.Services
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(azureAdConfiguration);
        builder.Services.AddAuthorization(options =>
        {
            foreach (var policyRole in policyRoles)
            {
                options.AddPolicy(policyRole.Policy, policy => policy.RequireRole(policyRole.Role));
            }
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
    }
    
}