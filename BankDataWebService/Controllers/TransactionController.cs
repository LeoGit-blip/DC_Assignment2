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

        [HttpPost]
        public IActionResult deposit([FromBody] Transaction transaction, int accountNumber)
        {
            var temp = DBManager.getByAccountNumber(accountNumber);

            if (transaction == null || transaction.transactionAmount <= 0)
            {
                return BadRequest(new { Message = "Amount cannot be negative" });
            }
            if(accountNumber == null)
            {
                return BadRequest(new { Message = "Amount didnt exist" });
            }
            return Ok(temp);
        }

        [HttpPost]
        public IActionResult withdraw([FromBody] Transaction transaction, int accountNumber)
        {
            var temp = DBManager.getByAccountNumber(accountNumber);

            if (transaction == null || transaction.transactionAmount <= 0)
            {
                return BadRequest(new { Message = "Amount cannot be negative" });
            }
            else if (accountNumber == null)
            {
                return BadRequest(new { Message = "Amount didnt exist" });
            }
            else
            {
                return Ok(temp);
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
