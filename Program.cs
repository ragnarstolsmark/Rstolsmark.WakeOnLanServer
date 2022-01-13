using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Rstolsmark.Owin.PasswordAuthentication;

var builder = WebApplication.CreateBuilder(args);
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
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });
}

builder.Services.AddSession();
var app = builder.Build();
var basedir = app.Environment.ContentRootPath;
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(basedir, "data"));
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
app.MapRazorPages();
app.MapControllers();
app.Run();