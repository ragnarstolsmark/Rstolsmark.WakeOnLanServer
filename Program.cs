using Rstolsmark.WakeOnLanServer.Configuration;
using Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;

var builder = WebApplication.CreateBuilder(args);
var policyRoles = new List<PolicyRole>();
builder.ConfigurePortForwarding(policyRoles);
var wakeOnLanRole = builder.Configuration.GetValue<string>("WakeOnLanRole");
var wakeOnLanAccessRequiresRole = !string.IsNullOrEmpty(wakeOnLanRole);
if (wakeOnLanAccessRequiresRole)
{
    policyRoles.Add( new PolicyRole(
        policy: "RequireWakeOnLanRole",
        role: wakeOnLanRole,
        folder: "/WakeOnLan")
    );
}
var mvcBuilder = builder.Services
    .AddRazorPages(options =>
    {
        options.Conventions.AddPageRoute("/WakeOnLan/Index", "/");
        foreach (var policyRole in policyRoles)
        {
            options.Conventions.AuthorizeFolder(policyRole.Folder, policyRole.Policy);
        }
    })
    .AddSessionStateTempDataProvider();
var azureAdConfiguration = builder.Configuration.GetSection("AzureAd");
if(azureAdConfiguration.Exists())
{
    builder.AddAzureAdAuthentication(mvcBuilder, azureAdConfiguration, policyRoles);
}
builder.Services.AddSession();
var app = builder.Build();
var basedir = app.Environment.ContentRootPath;
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(basedir, "data"));
app.ConfigureForwardedHeaders();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.ConfigurePasswordAuthentication();
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