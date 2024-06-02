using Microsoft.AspNetCore.Authorization;
using Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class AuthorizationConfiguration
{
    public const string RequirePortForwardingRole = "RequirePortForwardingRole";
    public const string RequireWakeOnLanRole = "RequireWakeOnLanRole";

    public static IReadOnlyCollection<PolicyRole> ConfigureAuthorization(this WebApplicationBuilder builder,
        PortForwardingSettings portForwardingSettings)
    {
        var policyRoles = new List<PolicyRole>();
        
        var wakeOnLanRole = builder.Configuration.GetValue<string>("WakeOnLanRole");
        var wakeOnLanAccessRequiresRole = !string.IsNullOrEmpty(wakeOnLanRole);
        if (wakeOnLanAccessRequiresRole)
            policyRoles.Add(new PolicyRole(
                RequireWakeOnLanRole,
                wakeOnLanRole,
                "/WakeOnLan")
            );
        
        var portForwardingAccessRequiresRole = !string.IsNullOrEmpty(portForwardingSettings.PortForwardingRole);
        if (portForwardingAccessRequiresRole) {
            policyRoles.Add(new PolicyRole(
                RequirePortForwardingRole,
                portForwardingSettings.PortForwardingRole,
                "/PortForwarding")
            );
        }
        
        builder.Services.AddAuthorization(options =>
        {
            foreach (var policyRole in policyRoles)
                options.AddPolicy(policyRole.Policy, policy => policy.RequireRole(policyRole.Role));

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        
        return policyRoles.AsReadOnly();
    }
}