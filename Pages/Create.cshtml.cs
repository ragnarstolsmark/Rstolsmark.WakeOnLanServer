using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Model;
using static Rstolsmark.WakeOnLanServer.Services.ComputerService;

namespace Rstolsmark.WakeOnLanServer.Pages;

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
        if (string.IsNullOrWhiteSpace(Computer.Name) || string.IsNullOrWhiteSpace(Computer.IP) || string.IsNullOrWhiteSpace(Computer.MAC) || string.IsNullOrWhiteSpace(Computer.SubnetMask))
        {
            TempData["Message"] = "You must fill out all fields.";
            return Page();
        }
        AddOrUpdateComputer(Computer);
        return RedirectToPage("/Index");
    }
}