using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;

public class DeleteComputer : PageModel
{
    private ComputerService _computerService;
    public DeleteComputer(ComputerService computerService)
    {
        _computerService = computerService;
    }

    [BindProperty(SupportsGet = true)]
    public string ComputerName { get; set; }

    public ActionResult OnGetAsync()
    {
        if (!_computerService.DoesComputerExist(ComputerName))
        {
            return NotFound();
        }
        return Page();
    }

    public ActionResult OnPostAsync()
    {
        if (!_computerService.DoesComputerExist(ComputerName))
        {
            return NotFound();
        }
        _computerService.Delete(ComputerName);
        return RedirectToPage("/WakeOnLan/Index");
    }
}
