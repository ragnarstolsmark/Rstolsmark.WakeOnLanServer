using System.Net;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

public class PortForwarding
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Protocol Protocol { get; set; }
    public IPAddress SourceIp { get; set; }
    public int? SourcePort { get; set; }
    public IPAddress DestinationIp { get; set; }
    public int DestinationPort { get; set; }
}