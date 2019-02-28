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
        public static Computer[] _computers = new Computer[]{
                new Computer {Name = "Ragnar", IP = "192.168.0.4", MAC = "FF:FF"},
                new Computer {Name = "Joar", IP = "192.168.0.5", MAC = "FF:AE"}
            };
        public Computer[] Computers { get; set; }
        public void OnGet()
        {
            Computers = _computers;
        }
        [BindProperty]
        public int ComputerToWake { get; set; }

        public void OnPost(){            
            _computers[ComputerToWake].Woken = true;
            Computers = _computers;
        }
    }
}
