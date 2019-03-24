using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WakeOnLanServer.Model;
namespace WakeOnLanServer.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Computer Computer { get; set; }
        public void OnGet()
        {
        }
    }
}