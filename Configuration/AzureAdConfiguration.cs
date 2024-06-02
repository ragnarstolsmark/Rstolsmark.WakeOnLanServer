using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class AzureAdConfiguration
{
    public static void AddAzureAdAuthentication(this WebApplicationBuilder builder, IMvcBuilder mvcBuilder,
        IConfigurationSection azureAdConfiguration)
    {
        mvcBuilder.AddMicrosoftIdentityUI();

        builder.Services
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(azureAdConfiguration);

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(azureAdConfiguration);

        builder.Services.AddAuthentication()
            .AddPolicyScheme("OpenIdConnect_Or_JWT", "OpenIdConnect_Or_JWT", options =>
            {
                options.ForwardDefaultSelector = context =>
                    context.Request.Path.StartsWithSegments("/api")
                        ? JwtBearerDefaults.AuthenticationScheme
                        : OpenIdConnectDefaults.AuthenticationScheme;
            });
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "OpenIdConnect_Or_JWT";
            options.DefaultChallengeScheme = "OpenIdConnect_Or_JWT";
        });
    }
    
}