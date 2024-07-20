namespace Rstolsmark.WakeOnLanServer.Services.PortForwarding;

public class PortForwardingWithIdDto : PortForwardingDto
{
    public PortForwardingWithIdDto(PortForwarding portForwarding) : base(portForwarding)
    {
        Id = portForwarding.Id;
    }
    public string Id { get; set; }
    
}