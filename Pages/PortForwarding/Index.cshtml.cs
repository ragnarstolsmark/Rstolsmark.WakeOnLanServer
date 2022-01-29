using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding;

public class Index : PageModel
{
    private readonly IPortForwardingService _portForwardingService;

    public Index(IPortForwardingService portForwardingService)
    {
        _portForwardingService = portForwardingService;
    }
    public Dictionary<string, Model.PortForwarding> PortForwardings { get; set; }
    public async Task OnGetAsync()
    {
        PortForwardings = await _portForwardingService.GetPortForwardingDictionary();
    }

    public async Task<IActionResult> OnPostEnableAsync(string portForwardingId)
    {
        await _portForwardingService.Enable(portForwardingId);
        return RedirectToPage("/PortForwarding/Index");
    }
    public async Task<IActionResult> OnPostDisableAsync(string portForwardingId)
    {
        await _portForwardingService.Disable(portForwardingId);
        return RedirectToPage("/PortForwarding/Index");
    }
}

public static class PortForwardingIndexExtensions
{
    public static string GetEnabledClass(this Model.PortForwarding portForwarding)
    {
        return portForwarding.Enabled ? "enabled" : string.Empty;
    }

    public static string GetEnabledMessage(this Model.PortForwarding portForwarding)
    {
        return portForwarding.Enabled ? "Enabled" : "Disabled";
    }
}