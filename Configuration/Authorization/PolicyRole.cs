namespace Rstolsmark.WakeOnLanServer.Configuration;

public class PolicyRole
{
    public string Policy { get; }
    public string Role { get; }
    public string Folder { get; set; }
    public PolicyRole(string policy, string role, string folder)
    {
        Policy = policy;
        Role = role;
        Folder = folder;
    }
    
}