using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesDemo.Models;

namespace SalesDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new Login());
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {
            User user = await Services.VaultService.Login(login);            
            return RedirectToAction("Index","Sandbox", new {session = user.Session, domain = user.Domain});
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
