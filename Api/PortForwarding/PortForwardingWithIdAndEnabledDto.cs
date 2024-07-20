namespace Rstolsmark.WakeOnLanServer.Services.PortForwarding;

public class PortForwardingWithIdAndEnabledDto : PortForwardingDto
{
    public PortForwardingWithIdAndEnabledDto(PortForwarding portForwarding) : base(portForwarding)
    {
        Id = portForwarding.Id;
        Enabled = portForwarding.Enabled;
    }
    public string Id { get; set; }
    public bool Enabled {get; set;}
}