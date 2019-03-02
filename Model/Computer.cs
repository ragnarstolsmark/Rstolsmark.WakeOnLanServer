using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace WakeOnLanServer.Model
{
    public class Computer
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
        public bool Woken {get; set;}

        public async Task Ping(){
            Ping pingSender = new Ping();
            //Send with a low timeout since it is on LAN and to prevent slow page load if computers are offline
            PingReply reply = await pingSender.SendPingAsync(IP, 50);
            Woken = reply.Status == IPStatus.Success;
        }
    }
}