using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WakeOnLanServer.Model;

namespace WakeOnLanServer.Pages {
	public class IndexModel : PageModel {
		public static Computer[] COMPUTERS => new Computer[]{
				new Computer {Name = "Ragnar", IP = "192.168.5.66", MAC = "18:31:BF:25:39:4A"},
				new Computer {Name = "Joar", IP = "192.168.5.156", MAC = "40:B0:76:0E:86:6B"}
			};
		public Dictionary<string, Computer> Computers { get; set; }
		public async Task OnGetAsync() {
			Computers = COMPUTERS.ToDictionary(c => c.Name);
			await PingAllComputers();
		}

		private Task PingAllComputers() {
			return Task.WhenAll(Computers.Values.Select(c => c.Ping()));
		}

		public async Task OnPostAsync(string computerToWake) {
			Computers = COMPUTERS.ToDictionary(c => c.Name);
			//Use a discard since we don't need to await the wake up since it will not start up fast enough to reply to the next ping anyway
			_ = Computers[computerToWake].WakeUp();
			await PingAllComputers();
		}
	}
}
