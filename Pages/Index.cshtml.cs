using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WakeOnLanServer.Model;

namespace WakeOnLanServer.Pages
{
    public class IndexModel : PageModel
    {
        public static Computer[] COMPUTERS => new Computer[]{
                new Computer {Name = "Ragnar", IP = "192.168.0.11", MAC = "3C:A9:F4:4E:32:8C"},
                new Computer {Name = "Joar", IP = "192.168.0.4", MAC = "3C:A9:F4:4E:32:8D"}
            };
        public  Dictionary<string, Computer> Computers { get; set; }
        public async Task OnGetAsync()
        {
            Computers = COMPUTERS.ToDictionary(c => c.Name);
            await PingAllComputers();
        }

        private Task PingAllComputers()
        {
            return Task.WhenAll(Computers.Values.Select(c => c.Ping()));
        }

        public async Task OnPostAsync(string computerToWake){
            Computers = COMPUTERS.ToDictionary(c=>c.Name);            
            //Use a discard since we don't need to await the wake up since it will not start up fast enough to reply to the next ping anyway
            _ = Computers[computerToWake].WakeUp();            
            await PingAllComputers();
        }
    }
}
