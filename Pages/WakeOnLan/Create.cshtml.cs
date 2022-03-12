using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;

public class CreateModel : PageModel
{
    private ComputerService _computerService;
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
        if (_computerService.DoesComputerExist(Computer.Name))
        {
            ModelState.AddModelError(nameof(Computer.Name), $"'{Computer.Name}' already exists.");
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }
        _computerService.AddComputer(Computer);
        return RedirectToPage("/WakeOnLan/Index");
    }
}
