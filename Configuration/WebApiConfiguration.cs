namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class WebApiConfiguration
{
    public static void ConfigureWebApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
    }
}