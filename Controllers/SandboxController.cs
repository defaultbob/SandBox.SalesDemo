using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using SalesDemo.Models;

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

            try
            {
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
                            ViewBag.Error = $"Request to {baseUri} was unsuccessful. Result {dynObj.responseStatus}. Error {dynObj.errors[0]}";
                            return View(user);
                        }
                        else
                        {
                            var userReturned = dynObj.users[0];
                            ViewBag.Name = userReturned.user.user_first_name__v + " " + userReturned.user.user_last_name__v;
                        }
                    }
                    else
                    {
                        ViewBag.Error = $"Request to {baseUri} was unsuccessful. Status code {response.StatusCode}";
                        return View(user);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(user);
        }

        public IActionResult Create(User user)
        {
            var request = new SandboxRequest(user);

            return View(request);
        }

        [HttpPost]
        public IActionResult Create(SandboxRequest request)
        {
            return this.NoContent();
        }

    }
}
