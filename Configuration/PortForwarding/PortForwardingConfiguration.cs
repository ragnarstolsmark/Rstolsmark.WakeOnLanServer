using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding.Backends;

namespace Rstolsmark.WakeOnLanServer.Configuration.PortForwarding;

public static class PortForwardingConfiguration
{
    public static PortForwardingSettings ConfigurePortForwarding(this WebApplicationBuilder builder)
    {
        var portForwardingConfiguration = builder.Configuration.GetSection("PortForwarding");
        PortForwardingSettings portForwardingSettings;
        if (portForwardingConfiguration.Exists())
        {
            builder.Services.AddSingleton<IValidator<PortForwardingDto>, PortForwardingDtoValidator>();
            portForwardingSettings = portForwardingConfiguration.Get<PortForwardingSettings>();
            switch (portForwardingSettings.Backend)
            {
                case PortForwardingBackend.Mock:
                    builder.Services.AddSingleton<IPortForwardingService, MockPortForwardingService>();
                    break;
                case PortForwardingBackend.Unifi:
                    if (string.IsNullOrEmpty(portForwardingSettings.UnifiClientOptions?.DefaultInterface))
                    {
                        throw new Exception("Unifi client needs a 'DefaultInterface' configured, to allow creation of port forwarding rules.");
                    }
                    builder.Services.AddMemoryCache();
                    builder.Services.AddSingleton(sp => 
                    {
                        var cache = sp.GetRequiredService<IMemoryCache>();
                        return new UnifiClient.UnifiClient(cache, portForwardingSettings.UnifiClientOptions);
                    });
                    builder.Services.AddSingleton<IPortForwardingService>(sp => 
                    {
                        var unifiClient = sp.GetRequiredService<UnifiClient.UnifiClient>();
                        return new UnifiPortForwardingService(unifiClient, portForwardingSettings.UnifiClientOptions.WanIp);
                    });
                    break;
            }
        } else
        {
            portForwardingSettings = new PortForwardingSettings
            {
                Backend = PortForwardingBackend.None
            };
        }
        builder.Services.AddSingleton(portForwardingSettings);
        return portForwardingSettings;
    }
}