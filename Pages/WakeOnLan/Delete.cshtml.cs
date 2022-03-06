using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model.ComputerService;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;

public class DeleteComputer : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string ComputerName { get; set; }

    public ActionResult OnGetAsync()
    {
        if (!DoesComputerExist(ComputerName))
        {
            return NotFound();
        }
        return Page();
    }

    public ActionResult OnPostAsync()
    {
        if (!DoesComputerExist(ComputerName))
        {
            return NotFound();
        }
        Delete(ComputerName);
        return RedirectToPage("/WakeOnLan/Index");
    }
}