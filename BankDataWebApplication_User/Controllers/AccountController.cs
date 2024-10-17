using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BankDataWebService.Models;

namespace BankDataWebApplication_User.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:46275/api/";
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync(BaseUrl + $"account/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var account = JsonSerializer.Deserialize<Account>(content, JsonOptions);
            return View(account);
        }
    }
}