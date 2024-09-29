using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Services.WakeOnLan;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;
namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;
public class EditComputerModel : PageModel
{
    private readonly ComputerService _computerService;
    public EditComputerModel(ComputerService computerService)
    {
        _computerService = computerService;
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
        var validator = new ComputerValidator(_computerService, ComputerName);
        var validationResult = validator.Validate(Computer);
        if(!validationResult.IsValid){
            validationResult.AddToModelState(ModelState);
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
