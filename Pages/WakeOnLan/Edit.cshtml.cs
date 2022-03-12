using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;
namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;
public class EditComputerModel : PageModel
{
    private ComputerService _computerService;
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
        if (ComputerName != Computer.Name && _computerService.DoesComputerExist(Computer.Name))
        {
            ModelState.AddModelError(nameof(Computer.Name), $"'{Computer.Name}' already exists.");
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
