using Rstolsmark.UnifiClient;

namespace Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;

public class PortForwardingSettings
{
    public UnifiClientOptions UnifiClientOptions { get; set; }
    public PortForwardingBackend Backend { get; set; }
    public string PortForwardingRole { get; set; }
}