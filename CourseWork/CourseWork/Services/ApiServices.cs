using CourseWork.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Services
{
    public class ApiServices
    {
        private const string url = "http://lotziko-001-site1.ftempurl.com";

        public async Task<(bool isSucceded, string message)> RegisterAsync(string email, string password, string confirmPassword)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var model = new RegisterModel()
                    {
                        Email = email,
                        Password = password,
                        ConfirmPassword = confirmPassword
                    };

                    var json = JsonConvert.SerializeObject(model);

                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(url + "/api/user/register", content);

                    return (response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                using (var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
                {
                    var model = new LoginModel()
                    {
                        Email = email,
                        Password = password
                    };

                    var json = JsonConvert.SerializeObject(model);

                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(url + "/api/user/login", content);

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                return (e.Message);
            }
        }

        public async Task<string> GetUsersAsync(string token)
        {
            try
            {
                using (var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync(url + "/api/user/getUsers");

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                return (e.Message);
            }
        }

        public async Task GetAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + "/api/values");
            var client = new HttpClient();
            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
        }
    }
}
