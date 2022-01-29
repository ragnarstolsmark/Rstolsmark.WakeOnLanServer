namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

public interface IPortForwardingService
{
    Task<IEnumerable<PortForwarding>> GetAll();
    Task<PortForwarding> AddPortforwarding(PortForwarding portForwarding);
    Task<PortForwarding> GetById(string id);
    Task Delete(string id);
}