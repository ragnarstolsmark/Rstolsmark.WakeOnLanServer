using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding;

[TypeFilter(typeof(BackendDownExceptionFilter))]
public class EditPortForwardingModel : PageModel
{
    private readonly IPortForwardingService _portForwardingService;
    private readonly IValidator<PortForwardingDto> _validator;

    public EditPortForwardingModel(IPortForwardingService portForwardingService, IValidator<PortForwardingDto> validator)
    {
        _portForwardingService = portForwardingService;
        _validator = validator;
    }
    [BindProperty(SupportsGet = true)]
    public string PortForwardingId { get; set; }
    [BindProperty] 
    public PortForwardingDto Dto { get; set; }
    public async Task<ActionResult> OnGetAsync()
    {
        var portForwarding = await _portForwardingService.GetById(PortForwardingId);
        if (portForwarding == null)
        {
            return NotFound();
        }
        Dto = new PortForwardingDto(portForwarding);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var validationResult = _validator.Validate(Dto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return Page();
        }
        var portForwarding = await _portForwardingService.GetById(PortForwardingId);
        if (portForwarding == null)
        {
            return NotFound();
        }
        await _portForwardingService.EditPortForwarding(PortForwardingId, new PortForwardingData(Dto));
        return RedirectToPage("/PortForwarding/Index");
    }
}