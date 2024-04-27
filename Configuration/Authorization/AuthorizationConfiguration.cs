using Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public class AuthorizationConfiguration
{
    public static IReadOnlyCollection<PolicyRole> GetPolicyRolesFromConfiguration(ConfigurationManager configuration, PortForwardingSettings portForwardingSettings)
    {
        var policyRoles = new List<PolicyRole>();
        var wakeOnLanRole = configuration.GetValue<string>("WakeOnLanRole");
        var wakeOnLanAccessRequiresRole = !string.IsNullOrEmpty(wakeOnLanRole);
        if (wakeOnLanAccessRequiresRole)
        {
            policyRoles.Add( new PolicyRole(
                policy: "RequireWakeOnLanRole",
                role: wakeOnLanRole,
                folder: "/WakeOnLan")
            );
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
        return policyRoles.AsReadOnly();
    }
}