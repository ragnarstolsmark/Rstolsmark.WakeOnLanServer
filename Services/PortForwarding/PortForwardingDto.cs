namespace Rstolsmark.WakeOnLanServer.Services.PortForwarding;

public class PortForwardingDto
{
    public PortForwardingDto()
    {
    }

    public PortForwardingDto(PortForwarding portForwarding)
    {
        Name = portForwarding.Name;
        Protocol = portForwarding.Protocol;
        SourceIp = portForwarding.SourceIp?.ToString();
        SourcePort = portForwarding.SourcePort;
        DestinationIp = portForwarding.DestinationIp.ToString();
        DestinationPort = portForwarding.DestinationPort;
    }
    public string Name { get; set; }
    public Protocol Protocol { get; set; }
    public string SourceIp { get; set; }
    public int SourcePort { get; set; }
    public string DestinationIp { get; set; }
    public int DestinationPort { get; set; }
}