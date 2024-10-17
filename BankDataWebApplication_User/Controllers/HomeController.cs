using BankDataWebApplication_User.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BankDataWebService.Data;
using BankDataWebService.Models;

namespace BankDataWebApplication_User.Controllers
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
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("api/user/account/{accountNumber}")]
        public IActionResult GetAccount(int accountNumber)
        {
            var account = DBManager.getByAccountNumber(accountNumber);
            if (account == null)
            {
                return NotFound();
            }
            return Json(account);
        }

        [HttpGet]
        [Route("api/user/transactions/{accountNumber}")]
        public IActionResult GetTransactions(int accountNumber)
        {
            var allTransactions = DBManager.getAllHistory();
            var accountTransactions = allTransactions
                .Where(t => t.transactionName != null &&
                            t.transactionName.Contains(accountNumber.ToString()))
                .ToList();
            return Json(accountTransactions);
        }

        [HttpPost]
        [Route("api/user/transaction")]
        public IActionResult CreateTransaction([FromBody] Transaction transaction)
        {
            // Extract the account number from the transaction name
            int accountNumber;
            if (!int.TryParse(transaction.transactionName, out accountNumber))
            {
                return BadRequest(new { message = "Invalid transaction name. It should contain the account number." });
            }

            var result = DBManager.insertTransaction(transaction);
            if (result)
            {
                // Update account balance
                var account = DBManager.getByAccountNumber(accountNumber);
                if (account != null)
                {
                    if (transaction.transactionType == "Deposit")
                    {
                        account.balance += transaction.transactionAmount;
                    }
                    else if (transaction.transactionType == "Withdraw")
                    {
                        account.balance -= transaction.transactionAmount;
                    }
                    DBManager.updateAccount(account);
                }
                else
                {
                    return NotFound(new { message = "Account not found" });
                }
            }
            return Json(new { success = result });
        }

        [HttpPut]
        [Route("api/user/account")]
        public IActionResult UpdateAccount([FromBody] Account account)
        {
            var result = DBManager.updateAccount(account);
            return Json(new { success = result });
        }
    }
}