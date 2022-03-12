using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan;

public class IndexModel : PageModel
{
    private ComputerService _computerService;
    public IndexModel(ComputerService computerService)
    {
        _computerService = computerService;
    }

    public Computer[] Computers { get; set; }
    public async Task OnGetAsync()
    {
        Computers = _computerService.GetAllComputers();
        await PingAllComputers();
    }

    private Task PingAllComputers()
    {
        return Task.WhenAll(Computers.Select(c => c.Ping()));
    }

    public IActionResult OnPost(string computerToWake)
    {
        //Use a discard since we don't need to await the wake up since it will not start up fast enough to reply to the next ping anyway
        _ = _computerService.GetComputerByName(computerToWake).WakeUp();
        TempData["Message"] = $"Magic packet sent to computer {@computerToWake}. It can take some time before it wakes up since it needs to boot first.";
        return RedirectToPage("/WakeOnLan/Index");
    }
}

public static class WakeOnLanIndexExtensions
{
    public static string GetAwakeClass(this Computer computer)
    {
        return computer.IsWoken() ? "enabled" : string.Empty;
    }

    public static string GetAwakeMessage(this Computer computer)
    {
        return computer.IsWoken() ? "Awake" : "Sleeping";
    }
}