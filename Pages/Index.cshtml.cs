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
                new Computer {Name = "Ragnar", IP = "192.168.0.4", MAC = "FF:FF"},
                new Computer {Name = "Joar", IP = "192.168.0.5", MAC = "FF:AE"}
            };
        public  Dictionary<string, Computer> Computers { get; set; }
        public void OnGet()
        {
            Computers = COMPUTERS.ToDictionary(c=>c.Name);
        }

        public void OnPost(string computerToWake){
            Computers = COMPUTERS.ToDictionary(c=>c.Name);            
            Computers[computerToWake].Woken = true;
        }
    }
}
