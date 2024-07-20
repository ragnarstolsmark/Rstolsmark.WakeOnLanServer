using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding;

[TypeFilter(typeof(BackendDownExceptionFilter))]
public class CreatePortForwardingModel : PageModel
{
    private readonly IPortForwardingService _portForwardingService;

    public CreatePortForwardingModel(IPortForwardingService portForwardingService)
    {
        _portForwardingService = portForwardingService;
    }

    [BindProperty]
    public PortForwardingDto Dto { get; set; }
    public void OnGet(string destinationIp)
    {
        Dto = new PortForwardingDto
        {
            SourceIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
            DestinationIp = destinationIp
        };
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        await _portForwardingService.AddPortForwarding(new PortForwardingData(Dto));
        return RedirectToPage("/PortForwarding/Index");
    }
}