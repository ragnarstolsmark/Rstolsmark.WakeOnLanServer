using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;
namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;
public class EditComputerModel : PageModel
{
    private readonly ComputerService _computerService;
    private readonly IValidator<Computer> _validator;
    public EditComputerModel(ComputerService computerService, IValidator<Computer> validator)
    {
        _computerService = computerService;
        _validator = validator;
    }

    [BindProperty(SupportsGet = true)]
    public string ComputerName { get; set; }
    [BindProperty] 
    public Computer Computer { get; set; }
    public ActionResult OnGetAsync()
    {
        Computer = _computerService.GetComputerByName(ComputerName);
        if (Computer == null)
        {
            return NotFound();
        }
        return Page();
    }
    public ActionResult OnPostAsync()
    {
        if (ComputerName != Computer.Name && _computerService.DoesComputerExist(Computer.Name))
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
        if (!_computerService.DoesComputerExist(ComputerName))
        {
            return NotFound();
        }
        _computerService.EditComputer(ComputerName, Computer);
        return RedirectToPage("/WakeOnLan/Index");
    }
}
