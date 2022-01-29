using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json.Serialization;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

public class Computer
{
    public string Name { get; set; }
    public string IP { get; set; }
    public string MAC { get; set; }
    public string SubnetMask { get; set; }
    [JsonIgnore]
    public bool Woken { get; set; }

    public async Task Ping()
    {
        Ping pingSender = new Ping();
        //Send with a low timeout since it is on LAN and to prevent slow page load if computers are offline
        PingReply reply = await pingSender.SendPingAsync(IP, 50);
        Woken = reply.Status == IPStatus.Success;
    }

    [JsonIgnore]
    public IPAddress BroadcastAddress
    {
        get
        {
            var address = IPAddress.Parse(IP);
            var mask = IPAddress.Parse(SubnetMask);
            uint ipAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            uint ipMaskV4 = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
            uint broadCastIpAddress = ipAddress | ~ipMaskV4;

            return new IPAddress(BitConverter.GetBytes(broadCastIpAddress));
        }
    }

    public async Task WakeUp()
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        s.EnableBroadcast = true;
        IPAddress broadcast = BroadcastAddress;
        byte[] prefix = new byte[] { 0xF_F, 0xF_F, 0xF_F, 0xF_F, 0xF_F, 0xF_F };
        var pureMac = MacToByteArrayToSend(MAC);
        byte[] sendbuf = prefix;
        for (int i = 1; i <= 16; i++)
        {
            sendbuf = sendbuf.Concat(pureMac).ToArray();
        }
        IPEndPoint ep = new IPEndPoint(broadcast, 9);
        await s.SendToAsync(sendbuf, SocketFlags.None, ep);
        byte[] MacToByteArrayToSend(string mac)
        {
            byte[] bytes = new byte[6];
            var hexStrings = mac.Split(":");
            for (int i = 0; i < 6; i++)
            {
                bytes[i] = Convert.ToByte(hexStrings[i], 16);
            }
            return bytes;
        }

    }
}