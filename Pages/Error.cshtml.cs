using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Rstolsmark.WakeOnLanServer.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ErrorModel : PageModel
{
    public string RequestId => HttpContext.TraceIdentifier;
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

}