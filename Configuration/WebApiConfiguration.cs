namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class WebApiConfiguration
{
    public static void ConfigureWebApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["requestId"] = context.HttpContext.TraceIdentifier;
            };
        });
    }
}