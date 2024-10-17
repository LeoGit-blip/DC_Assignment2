using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BankDataWebService.Models;

namespace BankDataWebApplication_Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:46275/api/";
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "user");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(content, JsonOptions);
            return View(users);
        }
    }
}