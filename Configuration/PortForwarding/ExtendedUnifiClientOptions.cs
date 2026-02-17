using Rstolsmark.UnifiClient;

namespace Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;

public class ExtendedUnifiClientOptions : UnifiClientOptions
{
    public string WanIp { get; set; }
}
