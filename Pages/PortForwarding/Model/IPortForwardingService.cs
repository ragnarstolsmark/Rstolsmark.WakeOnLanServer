namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

public interface IPortForwardingService
{
    Task<IEnumerable<PortForwarding>> GetAll();
    Task EditPortForwarding(string id, PortForwardingData portForwardingData);
    Task AddPortForwarding(PortForwardingData portForwardingData);
    Task<PortForwarding> GetById(string id);
    Task Delete(string id);
    Task Enable(string id);
    Task Disable(string id);

}