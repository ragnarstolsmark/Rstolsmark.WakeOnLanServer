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
}