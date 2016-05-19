using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using SalesDemo.Models;

namespace SalesDemo.Services
{
    public class VaultService
    {
        public async static Task<string> ValidateUser(User user)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var baseUri = $"https://{user.Domain}/api/v14.0/objects/users/me";
                    client.BaseAddress = new Uri(baseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", user.Session);
                    var response = await client.GetAsync(baseUri);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();

                        dynamic dynObj = JsonConvert.DeserializeObject(responseJson);

                        if (dynObj.responseStatus != "SUCCESS")
                        {
                            return $"Request to {baseUri} was unsuccessful. Result {dynObj.responseStatus}. Error {dynObj.errors[0]}";
                        }
                        else
                        {
                            var userReturned = dynObj.users[0];
                            return $"Welcome {userReturned.user.user_first_name__v} {userReturned.user.user_last_name__v} for domain {user.Domain}";
                        }
                    }
                    else
                    {
                        return $"Request to {baseUri} was unsuccessful. Status code {response.StatusCode}";
                    }
                }
            }
            catch (System.Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

    }
}