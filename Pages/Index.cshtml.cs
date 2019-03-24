using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WakeOnLanServer.Model;
using System.IO;

namespace WakeOnLanServer.Pages {
	public class IndexModel : PageModel {
		public static Computer[] COMPUTERS {
			get{
				if(!System.IO.File.Exists(@"computers.json")){
					return new Computer[]{};
				}
				return JsonConvert.DeserializeObject<Computer[]>(System.IO.File.ReadAllText(@"computers.json"));
			}
		}
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
