using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BankDataWebService.Models;
using BankDataWebService.Data;

namespace BankDataWebApplication_Admin.Controllers
{
    [Route("api/transactionmanagement")]
    public class TransactionController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/TransactionManagement/TransactionManagementPage.cshtml");
        }

        [HttpGet("all")]
        public IActionResult GetAllTransactions()
        {
            var transactions = DBManager.getAllHistory();
            return Ok(transactions);
        }

        [HttpGet("search")]
        public IActionResult SearchTransactions([FromQuery] string criteria, [FromQuery] string value)
        {
            var allTransactions = DBManager.getAllHistory();
            IEnumerable<Transaction> filteredTransactions;

            if (criteria.ToLower() == "name")
            {
                filteredTransactions = allTransactions.Where(t => t.transactionName != null &&
                    t.transactionName.ToLower().Contains(value.ToLower()));
            }
            else if (criteria.ToLower() == "type")
            {
                filteredTransactions = allTransactions.Where(t => t.transactionType != null &&
                    t.transactionType.ToLower().Contains(value.ToLower()));
            }
            else if (criteria.ToLower() == "amount")
            {
                if (double.TryParse(value, out double amount))
                {
                    filteredTransactions = allTransactions.Where(t => t.transactionAmount == amount);
                }
                else
                {
                    return BadRequest("Invalid amount value");
                }
            }
            else
            {
                return BadRequest("Invalid search criteria");
            }

            return Ok(filteredTransactions);
        }

        [HttpGet("sort")]
        public IActionResult SortTransactions([FromQuery] string sortBy, [FromQuery] bool ascending = true)
        {
            var allTransactions = DBManager.getAllHistory();
            IEnumerable<Transaction> sortedTransactions;

            switch (sortBy.ToLower())
            {
                case "date":
                    sortedTransactions = ascending
                        ? allTransactions.OrderBy(t => t.transactionTime)
                        : allTransactions.OrderByDescending(t => t.transactionTime);
                    break;
                case "amount":
                    sortedTransactions = ascending
                        ? allTransactions.OrderBy(t => t.transactionAmount)
                        : allTransactions.OrderByDescending(t => t.transactionAmount);
                    break;
                case "name":
                    sortedTransactions = ascending
                        ? allTransactions.OrderBy(t => t.transactionName)
                        : allTransactions.OrderByDescending(t => t.transactionName);
                    break;
                case "type":
                    sortedTransactions = ascending
                        ? allTransactions.OrderBy(t => t.transactionType)
                        : allTransactions.OrderByDescending(t => t.transactionType);
                    break;
                default:
                    return BadRequest("Invalid sort criteria");
            }

            return Ok(sortedTransactions);
        }
    }
}