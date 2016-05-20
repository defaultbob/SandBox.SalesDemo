using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesDemo.Models;

namespace SalesDemo.Controllers
{
    public class JobController : Controller
    {
        public async Task<IActionResult> Job(int id, string session, string domain)
        {
            User user = new User()
            {
                Domain = domain,
                Session = session
            };

            Job job = await Services.VaultService.GetJobStatus(user, id);

            return View(new Job());
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {
            User user = await Services.VaultService.Login(login);
            return RedirectToAction("Index", "Sandbox", new { session = user.Session, domain = user.Domain });
        }
    }
}