using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding;

[TypeFilter(typeof(BackendDownExceptionFilter))]
public class EditPortForwardingModel : PageModel
{
    private readonly IPortForwardingService _portForwardingService;

    public EditPortForwardingModel(IPortForwardingService portForwardingService)
    {
        _portForwardingService = portForwardingService;
    }
    [BindProperty(SupportsGet = true)]
    public string PortForwardingId { get; set; }
    [BindProperty] 
    public PortForwardingForm Form { get; set; }
    public async Task<ActionResult> OnGetAsync()
    {
        var portForwarding = await _portForwardingService.GetById(PortForwardingId);
        if (portForwarding == null)
        {
            return NotFound();
        }
        Form = new PortForwardingForm(portForwarding);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var portForwarding = await _portForwardingService.GetById(PortForwardingId);
        if (portForwarding == null)
        {
            return NotFound();
        }
        await _portForwardingService.EditPortForwarding(PortForwardingId, new PortForwardingData(Form));
        return RedirectToPage("/PortForwarding/Index");
    }
}