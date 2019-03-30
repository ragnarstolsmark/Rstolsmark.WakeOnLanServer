using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WakeOnLanServer.Model;
using System.IO;
using static WakeOnLanServer.Services.ComputerService;

namespace WakeOnLanServer.Pages {
	public class IndexModel : PageModel {
		public Dictionary<string, Computer> Computers { get; set; }
		public async Task OnGetAsync() {
			Computers = GetComputerDictionary();
			await PingAllComputers();
		}

		private Task PingAllComputers() {
			return Task.WhenAll(Computers.Values.Select(c => c.Ping()));
		}

		public IActionResult OnPost(string computerToWake) {
			Computers = GetComputerDictionary();
			//Use a discard since we don't need to await the wake up since it will not start up fast enough to reply to the next ping anyway
			_ = Computers[computerToWake].WakeUp();
			TempData["Message"] = $"Oppvåkningspakke sendt til datamaskinen {@computerToWake}. Det kan ta noe tid før den våkner siden den må skru seg på.";
			return RedirectToPage("/Index");
		}
	}
}
