using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using SalesDemo.Models;
using Microsoft.Extensions.Logging.Console;

namespace SalesDemo.Services
{
    public class VaultService
    {
        public async static Task<int> CreateSandboxRequest(User user, SandboxRequest request)
        {
            using (var client = new HttpClient())
            {
                var baseUri = $"https://{user.Domain}/api/v15.0/objects/sandbox";
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", user.Session);

                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("name", request.Name),
                        new KeyValuePair<string, string>("type", request.Type),
                        new KeyValuePair<string, string>("domain", request.TargetDomain)
                });

                var response = await client.PostAsync(baseUri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"response: {responseJson}");

                    dynamic dynObj = JsonConvert.DeserializeObject(responseJson);

                    if (dynObj.responseStatus != "SUCCESS")
                    {
                        throw new Exception($"Request to {baseUri} was unsuccessful. Result {dynObj.responseStatus}. Error {dynObj.errors[0]}");
                    }
                    else
                    {
                        return dynObj.job_id;
                    }
                }
                else
                {
                    throw new Exception($"Request to {baseUri} was unsuccessful. Status code {response.StatusCode}");
                }
            }
        }

        internal static async Task<Vault> Activate(Job job)
        {
            using (var client = new HttpClient())
            {
                var baseUri = $"https://{job.User.Domain}/api/v15.0/objects/sandbox/{job.Id}/activate";
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", job.User.Session);

                var response = await client.PostAsync(baseUri, null);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"response: {responseJson}");
                    dynamic dynObj = JsonConvert.DeserializeObject(responseJson);

                    if (dynObj.responseStatus != "SUCCESS")
                    {
                        throw new Exception($"Request to {baseUri} was unsuccessful. Result {dynObj.responseStatus}. Error {dynObj.errors[0]}");
                    }
                    else
                    {
                        return new Vault()
                        {
                            Id = dynObj.data[0].vault_id__v,
                            Dns = "https://" + dynObj.data[0].vault_dns__v,
                            Message = responseJson
                        };
                    }
                }
                else
                {
                    throw new Exception($"Request to {baseUri} was unsuccessful. Status code {response.StatusCode}");
                }
            }
        }

        internal static async Task<Job> GetJobStatus(User user, int id)
        {
            using (var client = new HttpClient())
            {
                var baseUri = $"https://{user.Domain}/api/v15.0/services/jobs/{id}";
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", user.Session);
                var response = await client.GetAsync(baseUri);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"response: {responseJson}");

                    dynamic dynObj = JsonConvert.DeserializeObject(responseJson);

                    if (dynObj.responseStatus != "SUCCESS")
                    {
                        return new Job()
                        {
                            Id = id,
                            Status = "FAILURE",
                            Message = responseJson
                        };
                        //throw new Exception($"Request to {baseUri} was unsuccessful. Result {dynObj.responseStatus}. Error {dynObj.errors[0]}");
                    }
                    else
                    {
                        return new Job()
                        {
                            Id = dynObj.data.id,
                            Status = dynObj.data.status,
                            Message = responseJson
                        };
                    }
                }
                else
                {
                    throw new Exception($"Request to {baseUri} was unsuccessful. Status code {response.StatusCode}");
                }
            }
        }

        public async static Task<User> Login(Login credentials)
        {
            using (var client = new HttpClient())
            {
                var baseUri = $"https://{credentials.Domain}/api/v14.0/auth?username={credentials.Username}&password={credentials.Password}";
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                var response = await client.PostAsync(baseUri, null);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"response: {responseJson}");

                    dynamic dynObj = JsonConvert.DeserializeObject(responseJson);

                    if (dynObj.responseStatus != "SUCCESS")
                    {
                        throw new Exception($"Request to {baseUri} was unsuccessful. Result {dynObj.responseStatus}. Error {dynObj.errors[0]}");
                    }
                    else
                    {
                        return new User((string)dynObj.sessionId, credentials.Domain);
                    }
                }
                else
                {
                    throw new Exception($"Request to {baseUri} was unsuccessful. Status code {response.StatusCode}");
                }
            }
        }

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
                        Console.WriteLine($"response: {responseJson}");

                        dynamic dynObj = JsonConvert.DeserializeObject(responseJson);

                        if (dynObj.responseStatus != "SUCCESS")
                        {
                            return $"Request to {baseUri} was unsuccessful. Result {dynObj.responseStatus}. Error {dynObj.errors[0]}";
                        }
                        else
                        {
                            var userReturned = dynObj.users[0];
                            return $"Welcome {userReturned.user.user_first_name__v}";
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