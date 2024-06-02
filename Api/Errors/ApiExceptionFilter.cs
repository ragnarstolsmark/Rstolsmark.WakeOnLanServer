using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;
namespace Rstolsmark.WakeOnLanServer.Api.Errors;
public class ApiExceptionFilter : IExceptionFilter
{

    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly ILogger<ApiExceptionFilter> _logger;
    public ApiExceptionFilter(ProblemDetailsFactory problemDetailsFactory, ILogger<ApiExceptionFilter> logger)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {
        ProblemDetails problemDetails;
        if (context.Exception is PortForwardingBackendTimeoutException)
        {
            _logger.LogError(context.Exception, "The port forwarding backend is down.");
            problemDetails = _problemDetailsFactory.CreateProblemDetails(
                context.HttpContext,
                StatusCodes.Status503ServiceUnavailable,
                "Port forwarding backend is down. Please try again later."
                );
            
        } else {
            _logger.LogError(context.Exception, "An unhandled exception has occurred while executing the request.");
            problemDetails = _problemDetailsFactory.CreateProblemDetails(
                context.HttpContext,
                StatusCodes.Status500InternalServerError,
                "An internal server error occured. Please check the logs for details for this requestId."
                );
        }
        context.Result = new ObjectResult(problemDetails){
            StatusCode = problemDetails.Status
        };
    }
}