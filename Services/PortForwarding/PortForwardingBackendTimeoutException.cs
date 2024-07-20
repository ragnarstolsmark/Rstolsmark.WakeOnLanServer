namespace Rstolsmark.WakeOnLanServer.Services.PortForwarding;

public class PortForwardingBackendTimeoutException : Exception
{
    public PortForwardingBackendTimeoutException(Exception innerException) : base("PortForwarding backend timed out. See inner exception for details.", innerException)
    {
        
    }
}