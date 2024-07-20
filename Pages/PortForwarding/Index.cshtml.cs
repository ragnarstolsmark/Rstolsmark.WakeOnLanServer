using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding;

[TypeFilter(typeof(BackendDownExceptionFilter))]
public class Index : PageModel
{
    private readonly IPortForwardingService _portForwardingService;

    public Index(IPortForwardingService portForwardingService)
    {
        _portForwardingService = portForwardingService;
    }
    public Dictionary<string, Services.PortForwarding.PortForwarding> PortForwardings { get; set; }
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
    public static string GetEnabledClass(this Services.PortForwarding.PortForwarding portForwarding)
    {
        return portForwarding.Enabled ? "enabled" : string.Empty;
    }

    public static string GetEnabledMessage(this Services.PortForwarding.PortForwarding portForwarding)
    {
        return portForwarding.Enabled ? "Enabled" : "Disabled";
    }
}