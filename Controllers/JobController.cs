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
        public IActionResult Index(string session, string domain)
        {
            var user = new User(session, domain);

            Job job = new Job{
                User = user
            };

            return View(job);
        }

        [HttpPost]
        public IActionResult Index(Job job)
        {
	        return RedirectToAction("Job", "Job", new { id = job.Id, session = job.User.Session, domain = job.User.Domain });
        }

        public async Task<IActionResult> Job(int id, string session, string domain)
        {
            var user = new User(session, domain);

            Job job = await Services.VaultService.GetJobStatus(user, id);
            job.User = user;

            return View(job);
        }

        [HttpPost]
        public async Task<IActionResult> Vault(Job job)
        {
            var vault = await Services.VaultService.Activate(job);
            
            return View(vault);
            return Redirect($"https://{vault.Dns}");
            //return RedirectToAction("Activate", "Job", new { id = job.Id, session = job.User.Session, domain = job.User.Domain });
        }

    }
}