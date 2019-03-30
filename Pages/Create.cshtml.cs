using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WakeOnLanServer.Model;
using static WakeOnLanServer.Services.ComputerService;

namespace WakeOnLanServer.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Computer Computer { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost(){
            if(string.IsNullOrWhiteSpace(Computer.Name) || string.IsNullOrWhiteSpace(Computer.IP) || string.IsNullOrWhiteSpace(Computer.MAC)){
                TempData["Message"] = "Du må fylle ut alle feltene";
                return Page();
            }
            AddOrUpdateComputer(Computer);
            return RedirectToPage("/Index");
        }
    }
}