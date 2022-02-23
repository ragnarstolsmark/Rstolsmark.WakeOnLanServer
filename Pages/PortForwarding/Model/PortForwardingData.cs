using System.Net;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

public class PortForwardingData
{
    public PortForwardingData()
    {
    }
    public PortForwardingData(PortForwardingForm form)
    {
        Name = form.Name;
        SourceIp = !string.IsNullOrWhiteSpace(form.SourceIp) ? IPAddress.Parse(form.SourceIp) : null;
        SourcePort = form.SourcePort;
        DestinationIp = IPAddress.Parse(form.DestinationIp);
        DestinationPort = form.DestinationPort;
    }
    public string Name { get; set; }
    public Protocol Protocol { get; set; }
    public IPAddress SourceIp { get; set; }
    public int SourcePort { get; set; }
    public IPAddress DestinationIp { get; set; }
    public int DestinationPort { get; set; }
}