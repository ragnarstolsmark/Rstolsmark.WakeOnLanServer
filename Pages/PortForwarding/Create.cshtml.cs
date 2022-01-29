using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding;

public class CreatePortForwardingModel : PageModel
{
    private readonly IPortForwardingService _portForwardingService;

    public CreatePortForwardingModel(IPortForwardingService portForwardingService)
    {
        _portForwardingService = portForwardingService;
    }
    [BindProperty]
    public string Name { get; set; }
    [BindProperty]
    public Protocol Protocol { get; set; }
    [BindProperty]
    public string SourceIp { get; set; }
    [BindProperty]
    public int? SourcePort { get; set; }
    [BindProperty(SupportsGet = true)]
    public string DestinationIp { get; set; }
    [BindProperty]
    public int DestinationPort { get; set; }
    public void OnGet()
    {
        SourceIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var _ = await _portForwardingService.AddPortforwarding(new Model.PortForwarding
        {
            Name = Name,
            SourceIp = !string.IsNullOrWhiteSpace(SourceIp) ? IPAddress.Parse(SourceIp) : null,
            SourcePort = SourcePort,
            DestinationIp = IPAddress.Parse(DestinationIp),
            DestinationPort = DestinationPort
        });
        return RedirectToPage("/PortForwarding/Index");
    }
}