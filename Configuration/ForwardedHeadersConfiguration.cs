using Microsoft.AspNetCore.HttpOverrides;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class ForwardedHeadersConfiguration
{
    public static void ConfigureForwardedHeaders(this WebApplication app)
    {
        var useForwardedHeaders = app.Configuration.GetValue<bool>("UseForwardedHeaders");
        if (useForwardedHeaders)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }
    }
}