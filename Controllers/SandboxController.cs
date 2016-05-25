using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using SalesDemo.Models;
using SalesDemo.Services;

namespace SalesDemo.Controllers
{
    public class SandboxController : Controller
    {
        public async Task<IActionResult> Index(string session, string domain)
        {
            var user = new User(session, domain);

            ViewBag.Domain = user.Domain;

            ViewBag.Error = await VaultService.ValidateUser(user);

            return View(user);
        }

        public IActionResult Create(User user)
        {
            var request = new SandboxRequest(user);

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SandboxRequest request)
        {
            if (ModelState.IsValid)
            {
                var jobId = await VaultService.CreateSandboxRequest(request.User, request);

                return RedirectToAction("Job", "Job", new { id = jobId, session = request.User.Session, domain = request.User.Domain });
            }
            else
            {
                return View(request);
            }
        }

    }
}
