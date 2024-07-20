using System.Net;

namespace Rstolsmark.WakeOnLanServer.Services.PortForwarding;

public class PortForwardingData
{
    public PortForwardingData()
    {
    }
    public PortForwardingData(PortForwardingDto dto)
    {
        Name = dto.Name;
        SourceIp = !string.IsNullOrWhiteSpace(dto.SourceIp) ? IPAddress.Parse(dto.SourceIp) : null;
        SourcePort = dto.SourcePort;
        DestinationIp = IPAddress.Parse(dto.DestinationIp);
        DestinationPort = dto.DestinationPort;
        Protocol = dto.Protocol;
    }
    public string Name { get; set; }
    public Protocol Protocol { get; set; }
    public IPAddress SourceIp { get; set; }
    public int SourcePort { get; set; }
    public IPAddress DestinationIp { get; set; }
    public int DestinationPort { get; set; }
}