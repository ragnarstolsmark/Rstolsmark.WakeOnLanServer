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
                    var unifiCache = new MemoryCache(new MemoryCacheOptions());
                    var unifiClient = new UnifiClient.UnifiClient(unifiCache, portForwardingSettings.UnifiClientOptions);
                    var unifiPortForwardingService = new UnifiPortForwardingService(unifiClient);
                    builder.Services.AddSingleton<IPortForwardingService>(unifiPortForwardingService);
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