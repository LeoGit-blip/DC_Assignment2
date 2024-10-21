using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BankDataWebService.Models;
using BankDataWebService.Data;

namespace BankDataWebApplication_User.Controllers
{
    [Route("api/transactionuser")]
    public class TransactionController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/TransactionHistory/TransactionHistoryPage.cshtml");
        }

        [HttpGet("history/{username}")]
        public IActionResult GetTransactionHistory(string username, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var userTransactions = DBManager.getTransactionsByUser(username);

            if (startDate.HasValue)
            {
                userTransactions = userTransactions.Where(t => t.transactionTime >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                userTransactions = userTransactions.Where(t => t.transactionTime <= endDate.Value).ToList();
            }

            return Json(userTransactions);
        }
    }
}