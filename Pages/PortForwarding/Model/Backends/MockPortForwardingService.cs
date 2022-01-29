using System.Net;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model.Backends;

public class MockPortForwardingService : IPortForwardingService
{
    private Dictionary<string, PortForwarding> _portForwardingConfigurations;
    private int _currentId;
    public MockPortForwardingService()
    {
        _portForwardingConfigurations = new Dictionary<string, PortForwarding>();
        _currentId = 1;
        AddPortForwarding(new PortForwarding
        {
            Name = "Ragnar hjemmekontor",
            Protocol = Protocol.TCP,
            SourceIp = IPAddress.Parse("84.212.6.118"),
            SourcePort = 3389,
            DestinationIp = IPAddress.Parse("192.168.5.68"),
            DestinationPort = 3389
        });
        AddPortForwarding(new PortForwarding
        {
            Name = "Knut hjemmekontor",
            Protocol = Protocol.TCP,
            SourceIp = IPAddress.Parse("84.212.6.119"),
            SourcePort = 3389,
            DestinationIp = IPAddress.Parse("192.168.5.69"),
            DestinationPort = 3389
        });
        AddPortForwarding(new PortForwarding
        {
            Name = "Hans hjemmekontor",
            Protocol = Protocol.TCP,
            SourceIp = IPAddress.Parse("84.212.6.120"),
            SourcePort = 3389,
            DestinationIp = IPAddress.Parse("192.168.5.70"),
            DestinationPort = 3389
        });
        AddPortForwarding(new PortForwarding
        {
            Name = "Anders hjemmekontor",
            Protocol = Protocol.TCP,
            SourceIp = IPAddress.Parse("84.212.6.121"),
            SourcePort = 3389,
            DestinationIp = IPAddress.Parse("192.168.5.71"),
            DestinationPort = 3389
        });
    }
    
    

    private PortForwarding AddPortForwarding(PortForwarding portForwarding)
    {
        portForwarding.Id = _currentId.ToString();
        _portForwardingConfigurations[portForwarding.Id] = portForwarding;
        _currentId++;
        return portForwarding;
    }

    Task<IEnumerable<PortForwarding>> IPortForwardingService.GetAll()
    {
        return Task.FromResult(_portForwardingConfigurations.Values.AsEnumerable());
    }

    Task<PortForwarding> IPortForwardingService.AddPortforwarding(PortForwarding portForwarding)
    {
        return Task.FromResult(AddPortForwarding(portForwarding));
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
}