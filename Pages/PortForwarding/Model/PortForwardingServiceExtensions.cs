using Rstolsmark.WakeOnLanServer.Services.PortForwarding;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

public static class PortForwardingServiceExtensions
{
    public static async Task<Dictionary<string, Services.PortForwarding.PortForwarding>> GetPortForwardingDictionary(this IPortForwardingService portForwardingService)
    {
        return (await portForwardingService.GetAll())
            .ToDictionary(p => p.Id);
    }
}