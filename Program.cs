﻿using Rstolsmark.WakeOnLanServer.Configuration;
using Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;
using Rstolsmark.WakeOnLanServer.Services;
using Serilog;

LoggingConfiguration.ConfigureBootstrapLogger();
Log.Information("Starting up");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureAzureKeyVault();
    builder.ConfigureLogger();
    builder.Services.AddSingleton<ProgramVersion>();
    var portForwardingSettings = builder.ConfigurePortForwarding();
    builder.ConfigureWakeOnLan();
    var azureAdConfiguration = builder.Configuration.GetSection("AzureAd");
    IReadOnlyCollection<PolicyRole> policyRoles;
    if (azureAdConfiguration.Exists())
    {
        policyRoles =
            builder.ConfigureAuthorization(portForwardingSettings);
    }
    else
    {
        policyRoles = [];
    }
    var enableApi = builder.Configuration.GetValue<bool>("EnableApi");
    if (enableApi)
    {
        builder.ConfigureWebApi();
    }
    var mvcBuilder = builder.ConfigureRazorPages(policyRoles);
    if (azureAdConfiguration.Exists())
    {
        builder.AddAzureAdAuthentication(mvcBuilder, azureAdConfiguration);
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
    app.UseRequestLocalization();
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
    if (enableApi)
    {
        app.MapControllers();
    }
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
