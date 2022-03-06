using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;
using static Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model.ComputerService;
namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;
public class EditComputerModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string ComputerName { get; set; }
    [BindProperty] 
    public Computer Computer { get; set; }
    public ActionResult OnGetAsync()
    {
        Computer = GetComputerByName(ComputerName);
        if (Computer == null)
        {
            return NotFound();
        }
        return Page();
    }
    public ActionResult OnPostAsync()
    {
        if (ComputerName != Computer.Name && DoesComputerExist(Computer.Name))
        {
            ModelState.AddModelError(nameof(Computer.Name), $"'{Computer.Name}' already exists.");
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (!DoesComputerExist(ComputerName))
        {
            return NotFound();
        }
        EditComputer(ComputerName, Computer);
        return RedirectToPage("/WakeOnLan/Index");
    }
}