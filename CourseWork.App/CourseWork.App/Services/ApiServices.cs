using CourseWork.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.App.Services
{
    public class ApiServices
    {
        private const string url = "http://lotziko-001-site1.ftempurl.com";

        public async Task<bool> RegisterAsync(string email, string password, string confirmPassword)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var model = new RegisterBindingModel()
                    {
                        Email = email,
                        Password = password,
                        ConfirmPassword = confirmPassword
                    };

                    var json = JsonConvert.SerializeObject(model);

                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(url + "/api/Account/Register", content);

                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var model = new LoginModel()
                    {
                        Email = email,
                        Password = password
                    };

                    var json = JsonConvert.SerializeObject(model);

                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(url + "/api/user/token", content);

                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task GetAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/values");
            var client = new HttpClient();
            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
        }
    }
}
