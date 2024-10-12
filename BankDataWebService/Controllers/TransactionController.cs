using Microsoft.AspNetCore.Mvc;
using BankDataWebService.Data;
using System.Security.Principal;
using BankDataWebService.Models;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        public IActionResult deposit([FromBody] Transaction transaction)
        {
            var temp = DBManager.getAllAcounts();

            if (account == null)
            {
                return BadRequest(new { Message = "Detect Account is null" });
            }
            else
            {
                account.balance += transaction.transactionAmount;
                transaction.transactionTime = DateTime.Now;
                transaction.transactionType = "DEPOSIT";
                return Ok(transaction);
            }
        }

        [HttpPost]
        public IActionResult withdraw([FromBody] Transaction transaction)
        {
            if (account == null)
            {
                return BadRequest(new { Message = "Detect Account is null" });
            }
            else
            {
                account.balance -= transaction.transactionAmount;
                transaction.transactionTime = DateTime.Now;
                transaction.transactionType = "WITHDRAW";
                return Ok(transaction);
            }
        }

        [HttpGet]
        public IActionResult History()
        {
            var temp = DBManager.getAllHistory();
            return Ok(temp);
        }
    }
}
