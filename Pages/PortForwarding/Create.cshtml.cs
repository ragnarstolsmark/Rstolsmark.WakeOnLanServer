using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding;

[TypeFilter(typeof(BackendDownExceptionFilter))]
public class CreatePortForwardingModel : PageModel
{
    private readonly IPortForwardingService _portForwardingService;
    private readonly IValidator<PortForwardingDto> _validator;

    public CreatePortForwardingModel(IPortForwardingService portForwardingService, IValidator<PortForwardingDto> validator)
    {
        _portForwardingService = portForwardingService;
        _validator = validator;
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
        var validationResult = _validator.Validate(Dto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return Page();
        }
        var _ = await _portForwardingService.AddPortForwarding(new PortForwardingData(Dto));
        return RedirectToPage("/PortForwarding/Index");
    }
}