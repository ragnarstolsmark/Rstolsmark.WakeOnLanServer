using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WakeOnLanServer.Model {
	public class Computer {
		public string Name { get; set; }
		public string IP { get; set; }
		public string MAC { get; set; }
		public bool Woken { get; set; }

		public async Task Ping() {
			Ping pingSender = new Ping();
			//Send with a low timeout since it is on LAN and to prevent slow page load if computers are offline
			PingReply reply = await pingSender.SendPingAsync(IP, 50);
			Woken = reply.Status == IPStatus.Success;
		}

		public async Task WakeUp() {
			Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			s.EnableBroadcast = true;
			IPAddress broadcast = IPAddress.Parse("255.255.255.255");
			byte[] prefix = new byte[] { 0xF_F, 0xF_F, 0xF_F, 0xF_F, 0xF_F, 0xF_F };
			var pureMac = MacToByteArrayToSend(MAC);
			byte[] sendbuf = prefix;
			for (int i = 1; i <= 16; i++) {
				sendbuf = sendbuf.Concat(pureMac).ToArray();
			}
			IPEndPoint ep = new IPEndPoint(broadcast, 9);
			await s.SendToAsync(sendbuf, SocketFlags.None, ep);
			byte[] MacToByteArrayToSend(string mac) {
				byte[] bytes = new byte[6];
				var hexStrings = mac.Split(":");
				for (int i = 0; i < 6; i++) {
					bytes[i] = Convert.ToByte(hexStrings[i], 16);
				}
				return bytes;
			}

		}
	}
}