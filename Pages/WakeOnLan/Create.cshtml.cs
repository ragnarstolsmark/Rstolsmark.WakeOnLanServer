using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;

public class CreateModel : PageModel
{
    private readonly ComputerService _computerService;
    private readonly IValidator<Computer> _validator;

    public CreateModel(ComputerService computerService, IValidator<Computer> validator)
    {
        _computerService = computerService;
        _validator = validator;
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
        if (_computerService.DoesComputerExist(Computer.Name))
        {
            ModelState.AddModelError(nameof(Computer.Name), $"'{Computer.Name}' already exists.");
        }
        var validationResult = _validator.Validate(Computer);
        if(!validationResult.IsValid){
            validationResult.AddToModelState(ModelState);
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }
        _computerService.AddComputer(Computer);
        return RedirectToPage("/WakeOnLan/Index");
    }
}
