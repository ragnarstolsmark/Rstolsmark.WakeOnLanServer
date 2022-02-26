using Rstolsmark.WakeOnLanServer.Configuration;
using Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;
using Serilog;

LoggingConfiguration.ConfigureBootstrapLogger();
Log.Information("Starting up");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureLogger();
    var policyRoles = new List<PolicyRole>();
    builder.ConfigurePortForwarding(policyRoles);
    builder.ConfigureWakeOnLan(policyRoles);
    var mvcBuilder = builder.ConfigureRazorPages(policyRoles);
    var azureAdConfiguration = builder.Configuration.GetSection("AzureAd");
    if (azureAdConfiguration.Exists())
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
    //Serve the static files before authentication and authorization to allow anonymous access.
    app.UseStaticFiles();
    app.ConfigureRequestLogging();
    app.ConfigurePasswordAuthentication();
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
    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
