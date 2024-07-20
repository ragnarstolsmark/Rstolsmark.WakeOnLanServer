using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

public class BackendDownExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is PortForwardingBackendTimeoutException)
        {
            context.Result = new RedirectToPageResult("/PortForwarding/BackendDown");
        }
    }
}