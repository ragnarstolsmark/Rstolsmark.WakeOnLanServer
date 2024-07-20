using System.Net;

namespace Rstolsmark.WakeOnLanServer.Services.PortForwarding;

public class PortForwarding
{
    public PortForwarding()
    {
        
    }
    public PortForwarding(string id, PortForwardingData portForwardingData)
    {
        Id = id;
        TransferPortforwardingData(portForwardingData);
        Enabled = true;
    }
    public string Id { get; set; }
    public string Name { get; set; }
    public Protocol Protocol { get; set; }
    public IPAddress SourceIp { get; set; }
    public int SourcePort { get; set; }
    public IPAddress DestinationIp { get; set; }
    public int DestinationPort { get; set; }
    public bool Enabled { get; set; }

    void TransferPortforwardingData(PortForwardingData portForwardingData)
    {
        Name = portForwardingData.Name;
        Protocol = portForwardingData.Protocol;
        SourceIp = portForwardingData.SourceIp;
        SourcePort = portForwardingData.SourcePort;
        DestinationIp = portForwardingData.DestinationIp;
        DestinationPort = portForwardingData.DestinationPort;
    }
}