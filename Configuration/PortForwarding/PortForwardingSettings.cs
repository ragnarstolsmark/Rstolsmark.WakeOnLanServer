namespace Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;

public class PortForwardingSettings
{
    public ExtendedUnifiClientOptions UnifiClientOptions { get; set; }
    public PortForwardingBackend Backend { get; set; }
    public string PortForwardingRole { get; set; }
}