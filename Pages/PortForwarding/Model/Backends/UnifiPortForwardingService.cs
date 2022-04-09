using System.Net;
using Rstolsmark.UnifiClient;
using UnifiPortForwarding = Rstolsmark.UnifiClient.PortForward;
using UnifiPortForwardingData = Rstolsmark.UnifiClient.PortForwardForm;
namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model.Backends;

public class UnifiPortForwardingService : IPortForwardingService
{
    private readonly UnifiClient.UnifiClient _unifiClient;

    public UnifiPortForwardingService(UnifiClient.UnifiClient unifiClient)
    {
        _unifiClient = unifiClient;
    }

    public async Task<IEnumerable<PortForwarding>> GetAll()
    {
        var portForwardSettings = await _unifiClient
            .GetPortForwardSettings()
            .WithTimeoutHandling();
        return portForwardSettings.Where(IsStandardPortForwarding).Select(MapUnifiPortForwardingToWakeOnLanServerPortForwarding);
    }
    // We only support port forwarding without ranges for source or destination
    private static bool IsStandardPortForwarding(UnifiPortForwarding unifiPortForward)
    {
        if (!string.IsNullOrEmpty(unifiPortForward.Source) && unifiPortForward.Source != "any" && !IPAddress.TryParse(unifiPortForward.Source, out _))
        {
            return false;
        }
        if (string.IsNullOrEmpty(unifiPortForward.Forward))
        {
            return false;
        }
        if (!IPAddress.TryParse(unifiPortForward.Forward, out _))
        {
            return false;
        }
        if (string.IsNullOrEmpty(unifiPortForward.ForwardPort))
        {
            return false;
        }
        if (!int.TryParse(unifiPortForward.ForwardPort, out _))
        {
            return false;
        }
        if (string.IsNullOrEmpty(unifiPortForward.DestinationPort))
        {
            return false;
        }
        if (!int.TryParse(unifiPortForward.DestinationPort, out _))
        {
            return false;
        }
        if (string.IsNullOrEmpty(unifiPortForward.Protocol))
        {
            return false;
        }
        if (unifiPortForward.Protocol != "tcp_udp" && !(Enum.TryParse(unifiPortForward.Protocol?.ToUpper(), out Protocol protocol) && Enum.IsDefined(protocol)))
        {
            return false;
        }
        return true;
    }

    private PortForwarding MapUnifiPortForwardingToWakeOnLanServerPortForwarding(UnifiPortForwarding unifiPortForward)
    {
        return new PortForwarding
        {
            Id = unifiPortForward.Id,
            Name = unifiPortForward.Name,
            Enabled = unifiPortForward.Enabled,
            Protocol = unifiPortForward.Protocol == "tcp_udp" ? Protocol.Any : Enum.Parse<Protocol>(unifiPortForward.Protocol.ToUpper()),
            SourceIp = string.IsNullOrEmpty(unifiPortForward.Source) || unifiPortForward.Source == "any" ? null : IPAddress.Parse(unifiPortForward.Source),
            SourcePort = int.Parse(unifiPortForward.DestinationPort),
            DestinationIp = IPAddress.Parse(unifiPortForward.Forward),
            DestinationPort = int.Parse(unifiPortForward.ForwardPort)
        };
    }

    private UnifiPortForwardingData MapWakeOnLanServerPortForwardingDataToUnifiPortForwardingData(PortForwardingData portForwardingData)
    {
        return new UnifiPortForwardingData
        {
            Name = portForwardingData.Name,
            Protocol = portForwardingData.Protocol == Protocol.Any ? "tcp_udp" : portForwardingData.Protocol.ToString().ToLower(),
            Source = portForwardingData.SourceIp?.ToString() ?? "any",
            ForwardPort = portForwardingData.DestinationPort.ToString(),
            Forward = portForwardingData.DestinationIp.ToString(),
            DestinationPort = portForwardingData.SourcePort.ToString()
        };
    }

    public async Task<PortForwarding> GetById(string id)
    {
        var unifiPortForward = await _unifiClient
            .GetPortForwardById(id)
            .WithTimeoutHandling();
        if (unifiPortForward != null && IsStandardPortForwarding(unifiPortForward))
        {
            return MapUnifiPortForwardingToWakeOnLanServerPortForwarding(unifiPortForward);
        }
        return null;
    }
    
    public async Task AddPortForwarding(PortForwardingData portForwardingData)
    {
        var unifiPortForwardingForm = MapWakeOnLanServerPortForwardingDataToUnifiPortForwardingData(portForwardingData);
        unifiPortForwardingForm.Enabled = true;
        await _unifiClient.CreatePortForwardSetting(unifiPortForwardingForm)
            .WithTimeoutHandling();
    }
    public async Task EditPortForwarding(string id, PortForwardingData portForwardingData)
    {
        var unifiPortForwardingForm = MapWakeOnLanServerPortForwardingDataToUnifiPortForwardingData(portForwardingData);
        await _unifiClient
            .EditPortForwardSetting(id, unifiPortForwardingForm)
            .WithTimeoutHandling();
    }

    public async Task Delete(string id)
    {
        await _unifiClient
            .DeletePortForwardSetting(id)
            .WithTimeoutHandling();
    }

    public async Task Enable(string id)
    {
        await _unifiClient
            .EditPortForwardSetting(id, new PortForwardForm {Enabled = true})
            .WithTimeoutHandling();
    }

    public async Task Disable(string id)
    { 
        await _unifiClient
            .EditPortForwardSetting(id, new PortForwardForm {Enabled = false})
            .WithTimeoutHandling();
    }
}
public static class UnifiExtensions
{
    public static async Task<T> WithTimeoutHandling<T>(this Task<T> action)
    {
        try
        {
            return await action;
        }
        catch (ClientTimoutException ex)
        {
            throw new PortForwardingBackendTimeoutException(ex);
        }
    }
    
    public static async Task WithTimeoutHandling(this Task action)
    {
        try
        {
            await action;
        }
        catch (ClientTimoutException ex)
        {
            throw new PortForwardingBackendTimeoutException(ex);
        }
    }
}