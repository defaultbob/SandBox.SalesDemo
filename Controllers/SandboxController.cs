using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

namespace SalesDemo.Controllers
{
    public class SandboxController : Controller
    {

        public async Task<IActionResult> Index(string session, string domain)
        {
            if (string.IsNullOrEmpty(session) || string.IsNullOrEmpty(domain))
            {
                return this.BadRequest("Must provide a session and a domain query string param - there is probably an issue with your Tab URL");
            }

            using (var client = new HttpClient())
            {
                var baseUri = "https://pm8.vaultdev.com/api/v14.0/objects/users/me";
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", session);
                var response = await client.GetAsync(baseUri);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    
                    dynamic dynObj = JsonConvert.DeserializeObject(responseJson);

                    if (dynObj.responseStatus != "SUCCESS")
                    {
                        return this.Unauthorized();
                    }
                    else
                    {
                        var user = dynObj.users[0];
                        ViewBag.Name = user.user.user_first_name__v + " " + user.user.user_last_name__v;
                    }
                }
            }

            return View();
        }

        public IActionResult SandboxRequest(string session, string domain)
        {
            if (string.IsNullOrEmpty(session) || string.IsNullOrEmpty(domain))
            {
                return this.BadRequest("Must provide a session and a domain query string param - there is probably an issue with your Tab URL");
            }

            var request = new SandboxRequest()
            {
                SessionId = session,
                SourceDomain = domain
            };

            return View(request);
        }

        [HttpPost]
        public IActionResult SandboxRequest(SandboxRequest request)
        {
            return this.NoContent();
        }

    }
}
