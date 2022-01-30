namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class WakeOnLanConfiguration
{
    public static void ConfigureWakeOnLan(this WebApplicationBuilder builder, List<PolicyRole> policyRoles){
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