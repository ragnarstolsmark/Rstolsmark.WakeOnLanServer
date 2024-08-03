using System.Net;

namespace Rstolsmark.WakeOnLanServer.Services.PortForwarding.Backends;

public class MockPortForwardingService : IPortForwardingService
{
    private Dictionary<string, PortForwarding> _portForwardingConfigurations;
    private int _currentId;
    public MockPortForwardingService()
    {
        _portForwardingConfigurations = new Dictionary<string, PortForwarding>();
        _currentId = 1;
        AddPortForwarding(new PortForwardingData
        {
            Name = "Ragnar hjemmekontor",
            Protocol = Protocol.TCP,
            SourceIp = IPAddress.Parse("84.212.6.118"),
            SourcePort = 3389,
            DestinationIp = IPAddress.Parse("192.168.5.68"),
            DestinationPort = 3389
        });
        AddPortForwarding(new PortForwardingData
        {
            Name = "Knut hjemmekontor",
            Protocol = Protocol.TCP,
            SourceIp = IPAddress.Parse("84.212.6.119"),
            SourcePort = 3389,
            DestinationIp = IPAddress.Parse("192.168.5.69"),
            DestinationPort = 3389,
        });
        AddPortForwarding(new PortForwardingData
        {
            Name = "Hans hjemmekontor",
            Protocol = Protocol.TCP,
            SourceIp = IPAddress.Parse("84.212.6.120"),
            SourcePort = 3389,
            DestinationIp = IPAddress.Parse("192.168.5.70"),
            DestinationPort = 3389,
        });
        var andersHomeOffice = AddPortForwarding(new PortForwardingData
        {
            Name = "Anders hjemmekontor",
            Protocol = Protocol.TCP,
            SourceIp = IPAddress.Parse("84.212.6.121"),
            SourcePort = 3389,
            DestinationIp = IPAddress.Parse("192.168.5.71"),
            DestinationPort = 3389,
        });
        Disable(andersHomeOffice.Id);
    }

    private PortForwarding AddPortForwarding(PortForwardingData portForwardingData)
    {
        var portForwarding = new PortForwarding(_currentId.ToString(), portForwardingData);
        _portForwardingConfigurations[portForwarding.Id] = portForwarding;
        _currentId++;
        return portForwarding;
    }

    Task<IEnumerable<PortForwarding>> IPortForwardingService.GetAll()
    {
        return Task.FromResult(_portForwardingConfigurations.Values.AsEnumerable());
    }

    public Task EditPortForwarding(string id, PortForwardingData portForwardingData)
    {
        if (_portForwardingConfigurations.ContainsKey(id))
        {
            var editedPortForwarding = _portForwardingConfigurations[id];
            editedPortForwarding.Name = portForwardingData.Name;
            editedPortForwarding.Protocol = portForwardingData.Protocol;
            editedPortForwarding.SourceIp = portForwardingData.SourceIp;
            editedPortForwarding.SourcePort = portForwardingData.SourcePort;
            editedPortForwarding.DestinationIp = portForwardingData.DestinationIp;
            editedPortForwarding.DestinationPort = portForwardingData.DestinationPort;
        }
        return Task.CompletedTask;
    }

    Task<PortForwarding> IPortForwardingService.AddPortForwarding(PortForwardingData portForwardingData)
    {
        var portForwarding = AddPortForwarding(portForwardingData);
        return Task.FromResult(portForwarding);
    }

    public Task<PortForwarding> GetById(string id)
    {
        PortForwarding portForwarding = null;
        if (_portForwardingConfigurations.ContainsKey(id))
        {
            portForwarding = _portForwardingConfigurations[id];
        }
        return Task.FromResult(portForwarding);
    }

    public Task Delete(string id)
    {
        if (_portForwardingConfigurations.ContainsKey(id))
        {
            _portForwardingConfigurations.Remove(id);
        }
        return Task.CompletedTask;
    }

    public Task Enable(string id)
    {
        if (_portForwardingConfigurations.ContainsKey(id))
        {
            _portForwardingConfigurations[id].Enabled = true;
        }
        return Task.CompletedTask;
    }
    public Task Disable(string id)
    {
        if (_portForwardingConfigurations.ContainsKey(id))
        {
            _portForwardingConfigurations[id].Enabled = false;
        }
        return Task.CompletedTask;
    }
}