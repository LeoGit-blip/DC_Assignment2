using BankDataWebApplication_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BankDataWebService.Data;
using BankDataWebService.Models;
using ErrorViewModel = BankDataWebApplication_Admin.Models.ErrorViewModel;

namespace BankDataWebApplication_Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("api/admin/accounts")]
        public IActionResult GetAllAccounts()
        {
            var accounts = DBManager.getAllAcounts();
            return Json(accounts);
        }

        [HttpGet]
        [Route("api/admin/users")]
        public IActionResult GetAllUsers()
        {
            var users = DBManager.getAllUsers();
            return Json(users);
        }

        [HttpGet]
        [Route("api/admin/transactions")]
        public IActionResult GetAllTransactions()
        {
            var transactions = DBManager.getAllHistory();
            return Json(transactions);
        }

        [HttpPost]
        [Route("api/admin/account")]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            var result = DBManager.insertAccount(account);
            return Json(new { success = result });
        }

        [HttpPost]
        [Route("api/admin/user")]
        public IActionResult CreateUser([FromBody] User user)
        {
            var result = DBManager.insertUser(user);
            return Json(new { success = result });
        }

        [HttpPut]
        [Route("api/admin/account")]
        public IActionResult UpdateAccount([FromBody] Account account)
        {
            var result = DBManager.updateAccount(account);
            return Json(new { success = result });
        }

        [HttpPut]
        [Route("api/admin/user")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            var result = DBManager.updateUser(user);
            return Json(new { success = result });
        }

        [HttpDelete]
        [Route("api/admin/account/{accountNumber}")]
        public IActionResult DeleteAccount(int accountNumber)
        {
            var account = DBManager.getByAccountNumber(accountNumber);
            if (account == null)
            {
                return NotFound();
            }
            var result = DBManager.deleteAccount(account);
            return Json(new { success = result });
        }

        [HttpDelete]
        [Route("api/admin/user/{email}")]
        public IActionResult DeleteUser(string email)
        {
            var user = DBManager.getByUserEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            var result = DBManager.deleteUser(user);
            return Json(new { success = result });
        }
    }
}