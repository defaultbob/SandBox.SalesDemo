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
            var user = new User()
            {
                Session = session,
                Domain = domain
            };

            if (string.IsNullOrEmpty(user.Session) || string.IsNullOrEmpty(user.Domain))
            {
                return this.BadRequest("Must provide a session and a domain query string param - there is probably an issue with your Tab URL");
            }

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
            var jobId = await VaultService.CreateSandboxRequest(request.User, request);
            
            return RedirectToAction("Job", new {id = jobId});
        }

    }
}
