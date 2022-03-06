using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;
using static Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model.ComputerService;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;

public class CreateModel : PageModel
{
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
        if (DoesComputerExist(Computer.Name))
        {
            ModelState.AddModelError(nameof(Computer.Name), $"'{Computer.Name}' already exists.");
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }
        AddComputer(Computer);
        return RedirectToPage("/WakeOnLan/Index");
    }
}