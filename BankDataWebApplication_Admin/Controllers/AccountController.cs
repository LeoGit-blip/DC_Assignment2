using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BankDataWebService.Models;

namespace BankDataWebApplication_Admin.Controllers
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

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(BaseUrl + "account");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var accounts = JsonSerializer.Deserialize<List<Account>>(content, JsonOptions);
            return View(accounts);
        }
    }
}