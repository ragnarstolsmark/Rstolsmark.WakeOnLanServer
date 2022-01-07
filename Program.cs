using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Rstolsmark.Owin.PasswordAuthentication;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddRazorPages()
    .AddSessionStateTempDataProvider();
var azureAdConfiguration = builder.Configuration.GetSection("AzureAd");
if(azureAdConfiguration.Exists()){
    builder.Services
        .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(azureAdConfiguration);
    builder.Services.AddAuthorization(options =>
    {
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

if (azureAdConfiguration.Exists())
{
    app.UseAuthentication();
    app.UseAuthorization();
}
app.UseStaticFiles();
app.UseSession();
app.MapRazorPages();
app.Run();