using System.Text.Json.Serialization;

namespace Rstolsmark.WakeOnLanServer.Services.PortForwarding;
[JsonConverter(typeof(JsonStringEnumConverter<Protocol>))]
public enum Protocol
{
    TCP,
    UDP,
    Any
}