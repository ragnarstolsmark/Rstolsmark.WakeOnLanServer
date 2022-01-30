using Rstolsmark.Owin.PasswordAuthentication;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class PasswordAuthenticationConfigurationExtensions
{
    public static void ConfigurePasswordAuthentication(this WebApplication app)
    {
        var passwordAuthenticationOptions = app.Configuration.GetSection("PasswordAuthenticationOptions").Get<PasswordAuthenticationOptions>();
        if (!string.IsNullOrEmpty(passwordAuthenticationOptions?.HashedPassword))
        {
            app.UseOwin(pipeline =>
            {
                pipeline.UsePasswordAuthentication(passwordAuthenticationOptions);
            });
        }
    }
}