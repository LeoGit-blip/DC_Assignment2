using Microsoft.AspNetCore.Mvc;
using BankDataWebService.Data;
using BankDataWebService.Models;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        [HttpPost]
        public IActionResult createTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest(new { Message = "Detect transaction is null" });
            }
            else
            {
                DBManager.insertTransaction(transaction);
                var response = new { Message = "Transaction create successfully" };
                return new ObjectResult(response)
                {
                    StatusCode = 200,
                    ContentTypes = { "application/json" }
                };
            }
        }

        [HttpGet]
        public IActionResult History()
        {
            var history = DBManager.getAllHistory();
            if (history == null)
            {
                return NotFound(new { Message = "The list is empty" });
            }
            return Ok(history);
        }

        [HttpPost]
        public IActionResult deposit([FromBody] Transaction transaction, int account1, int account2)
        {
            var temp1 = DBManager.getByAccountNumber(account1);
            var temp2 = DBManager.getByAccountNumber(account2);

            if (temp1 == null || temp2 == null)
            {
                return BadRequest(new { Message = "Account cannot be found" });
            }
            if(transaction == null || transaction.transactionAmount <= 0)
            {
                return BadRequest(new { Message = "Amount cannot be less than zero" });
            }
            if (temp1.balance < transaction.transactionAmount)
            {
                return BadRequest(new { Message = "Insufficient balance" });
            }
            else
            {
                temp1.balance -= transaction.transactionAmount;
                temp2.balance += transaction.transactionAmount;
                DBManager.insertTransaction(transaction);
            }
            return Ok(temp2);
        }

        [HttpPost]
        public IActionResult withdraw([FromBody] Transaction transaction, int accountNumber)
        {
            var temp = DBManager.getByAccountNumber(accountNumber);

            if (temp == null)
            {
                return BadRequest(new { Message = "Account cannot be found" });
            }
            if (transaction == null || transaction.transactionAmount <= 0)
            {
                return BadRequest(new { Message = "Amount withdraw cannot be less than zero" });
            }
            if(temp.balance < transaction.transactionAmount)
            {
                return BadRequest(new { Message = "Insufficient balance" });
            }
            else
            {
                temp.balance += transaction.transactionAmount;
                DBManager.insertTransaction(transaction);
            }
            return Ok(temp);
        }
    }
}
