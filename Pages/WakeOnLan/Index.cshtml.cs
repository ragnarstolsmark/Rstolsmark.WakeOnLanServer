using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Rstolsmark.WakeOnLanServer.Services.WakeOnLan;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;

public class IndexModel : PageModel
{
    private ComputerService _computerService;
    private readonly IStringLocalizer<IndexModel> _localizer;

    public IndexModel(ComputerService computerService, IStringLocalizer<IndexModel> localizer)
    {
        _computerService = computerService;
        _localizer = localizer;
    }

    public ComputerWithAwakeDto[] Computers { get; set; }
    public async Task OnGetAsync()
    {
        Computers = await _computerService.GetAllComputersWithAwakeStatus();
    }

    public IActionResult OnPost(string computerToWake)
    {
        //Use a discard since we don't need to await the wake up since it will not start up fast enough to reply to the next ping anyway
        _ = _computerService.GetComputerByName(computerToWake).WakeUp();
        TempData["Message"] = _localizer["Magic packet sent to computer {0}. It can take some time before it wakes up since it needs to boot first.", computerToWake].Value;
        return RedirectToPage("/WakeOnLan/Index");
    }
}

public static class WakeOnLanIndexExtensions
{
    public static string GetAwakeClass(this ComputerWithAwakeDto computer)
    {
        return computer.Awake ? "enabled" : string.Empty;
    }

    public static string GetAwakeMessage(this ComputerWithAwakeDto computer)
    {
        return computer.Awake ? "Awake" : "Sleeping";
    }
}