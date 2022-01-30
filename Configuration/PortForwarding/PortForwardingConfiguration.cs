using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model.Backends;

namespace Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;

public static class PortForwardingConfiguration
{
    public static void ConfigurePortForwarding(this WebApplicationBuilder builder, List<PolicyRole> policyRoles)
    {
        
        var portForwardingConfiguration = builder.Configuration.GetSection("PortForwarding");
        PortForwardingSettings portForwardingSettings;
        if (portForwardingConfiguration.Exists())
        {
            portForwardingSettings = portForwardingConfiguration.Get<PortForwardingSettings>();
            switch (portForwardingSettings.Backend)
            {
                case PortForwardingBackend.Mock:
                    builder.Services.AddSingleton<IPortForwardingService, MockPortForwardingService>();
                    break;
            }
        } else
        {
            portForwardingSettings = new PortForwardingSettings
            {
                Backend = PortForwardingBackend.None
            };
        }
        var portForwardingAccessRequiresRole = !string.IsNullOrEmpty(portForwardingSettings.PortForwardingRole);
        if (portForwardingAccessRequiresRole)
        {
            policyRoles.Add(new PolicyRole(
                policy: "RequirePortForwardingRole",
                role: portForwardingSettings.PortForwardingRole,
                folder: "/PortForwarding")
            );
        }
        builder.Services.AddSingleton(portForwardingSettings);
    }
}