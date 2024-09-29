using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Services.WakeOnLan;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;

public class CreateModel : PageModel
{
    private readonly ComputerService _computerService;

    public CreateModel(ComputerService computerService)
    {
        _computerService = computerService;
    }

    [BindProperty]
    public Computer Computer { get; set; }
    public void OnGet()
    {
        Computer = new Computer
        {
            SubnetMask = "255.255.255.0"
        };
    }
    public IActionResult OnPost()
    {
        var validator = new ComputerValidator(_computerService);
        var validationResult = validator.Validate(Computer);
        if(!validationResult.IsValid){
            validationResult.AddToModelState(ModelState);
            return Page();
        }
        _computerService.AddComputer(Computer);
        return RedirectToPage("/WakeOnLan/Index");
    }
}
