using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding;

public class DeletePortForwarding : PageModel
{
    private readonly IPortForwardingService _portForwardingService;

    public DeletePortForwarding(IPortForwardingService portForwardingService)
    {
        _portForwardingService = portForwardingService;
    }
    [BindProperty(SupportsGet = true)]
    public string PortForwardingId { get; set; }

    public string Name { get; set; }
    public async Task<ActionResult> OnGetAsync()
    {
        var portForwarding = await _portForwardingService.GetById(PortForwardingId);
        if (portForwarding == null)
        {
            return NotFound();
        }
        Name = portForwarding.Name;
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        await _portForwardingService.Delete(PortForwardingId);
        return RedirectToPage("/PortForwarding/Index");
    }
}