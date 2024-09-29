using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Rstolsmark.WakeOnLanServer.Services.WakeOnLan;

public class Computer
{
    public Computer()
    {
    }
    public Computer(Computer computer)
    {
        Name = computer.Name;
        IP = computer.IP;
        MAC = computer.MAC;
        SubnetMask = computer.SubnetMask;
    }

    public string Name { get; set; }
    public string IP { get; set; }
    public string MAC { get; set; }
    public string SubnetMask { get; set; }

    /// <summary>
    /// Ping the computer
    /// </summary>
    /// <returns>A bool indicating if the ping succeeded and therefore the computer should be considered awake</returns>
    public async Task<bool> Ping()
    {
        Ping pingSender = new Ping();
        //Send with a low timeout since it is on LAN and to prevent slow page load if computers are offline
        PingReply reply = await pingSender.SendPingAsync(IP, 50);
        return reply.Status == IPStatus.Success;
    }
    
    private IPAddress GetBroadcastAddress()
    {
        var address = IPAddress.Parse(IP);
        var mask = IPAddress.Parse(SubnetMask);
        uint ipAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
        uint ipMaskV4 = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
        uint broadCastIpAddress = ipAddress | ~ipMaskV4;

        return new IPAddress(BitConverter.GetBytes(broadCastIpAddress));
    }

    private PhysicalAddress GetPhysicalAddress()
    {
        return PhysicalAddress.Parse(MAC);
    }

    public void StandardizeMacAddress()
    {
        MAC = string.Join (":", GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));
    }

    public async Task WakeUp()
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        s.EnableBroadcast = true;
        IPAddress broadcast = GetBroadcastAddress();
        byte[] prefix = new byte[] { 0xF_F, 0xF_F, 0xF_F, 0xF_F, 0xF_F, 0xF_F };
        var pureMac = GetPhysicalAddress().GetAddressBytes();
        byte[] sendbuf = prefix;
        for (int i = 1; i <= 16; i++)
        {
            sendbuf = sendbuf.Concat(pureMac).ToArray();
        }
        IPEndPoint ep = new IPEndPoint(broadcast, 9);
        await s.SendToAsync(sendbuf, SocketFlags.None, ep);
    }
}