using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Rstolsmark.Owin.PasswordAuthentication;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model.Backends;

var builder = WebApplication.CreateBuilder(args);
var portForwardingConfiguration = builder.Configuration.GetSection("PortForwarding");
PortForwardingSettings portForwardingSettings;
if (portForwardingConfiguration.Exists())
{
    portForwardingSettings = portForwardingConfiguration.Get<PortForwardingSettings>();
    switch (portForwardingSettings.Backend)
    {
        case PortForwardingBackend.Mock:
            builder.Services.AddSingleton<IPortForwardingService, MockPortForwardingService>();
            break;
    }
} else
{
    portForwardingSettings = new PortForwardingSettings
    {
        Backend = PortForwardingBackend.None
    };
}

var portForwardingAccessRequiresRole = !string.IsNullOrEmpty(portForwardingSettings.PortForwardingRole);
const string requirePortForwardingRolePolicy = "RequirePortForwardingRole";
builder.Services.AddSingleton(portForwardingSettings);
var wakeOnLanRole = builder.Configuration.GetValue<string>("WakeOnLanRole");
var wakeOnLanAccessRequiresRole = !string.IsNullOrEmpty(wakeOnLanRole);
const string requireWakeOnLanRolePolicy = "RequireWakeOnLanRole";
var mvcBuilder = builder.Services
    .AddRazorPages(options =>
    {
        options.Conventions.AddPageRoute("/WakeOnLan/Index", "/");
        if (wakeOnLanAccessRequiresRole)
        {
            options.Conventions.AuthorizeFolder("/WakeOnLan", requireWakeOnLanRolePolicy);
        }

        if (portForwardingAccessRequiresRole)
        {
            options.Conventions.AuthorizeFolder("/PortForwarding", requirePortForwardingRolePolicy);
        }
    })
    .AddSessionStateTempDataProvider();
var azureAdConfiguration = builder.Configuration.GetSection("AzureAd");
if(azureAdConfiguration.Exists())
{
    mvcBuilder.AddMicrosoftIdentityUI();
    builder.Services
        .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(azureAdConfiguration);
    builder.Services.AddAuthorization(options =>
    {
        if (wakeOnLanAccessRequiresRole)
        {
            options.AddPolicy(requireWakeOnLanRolePolicy,
                policy => policy.RequireRole(wakeOnLanRole!));
        }

        if (portForwardingAccessRequiresRole)
        {
            options.AddPolicy(requirePortForwardingRolePolicy,
                policy => policy.RequireRole(portForwardingSettings.PortForwardingRole));
        }
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });
}

builder.Services.AddSession();
var app = builder.Build();
var basedir = app.Environment.ContentRootPath;
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(basedir, "data"));
var useForwardedHeaders = app.Configuration.GetValue<bool>("UseForwardedHeaders");
if (useForwardedHeaders)
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
var passwordAuthenticationOptions = app.Configuration.GetSection("PasswordAuthenticationOptions").Get<PasswordAuthenticationOptions>();
if (!string.IsNullOrEmpty(passwordAuthenticationOptions?.HashedPassword))
{
    app.UseOwin(pipeline =>
    {
        pipeline.UsePasswordAuthentication(passwordAuthenticationOptions);
    });
}
//Serve the static files before authentication and authorization to allow anonymous access.
app.UseStaticFiles();
if (azureAdConfiguration.Exists())
{
    app.UseAuthentication();
    app.UseAuthorization();
}
app.UseSession();
app.UseStatusCodePagesWithReExecute("/Errors/{0}");
app.MapRazorPages();
app.MapControllers();
app.Run();