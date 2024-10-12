using BankDataWebService.Data;
using BankDataWebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            if (account == null)
            {
                return BadRequest(new { Message = "Detect Account is null" });
            }
            else
            {
                DBManager.insertAccount(account);
                var response = new { Message = "Account create successfully" };
                return new ObjectResult(response)
                {
                    StatusCode = 200,
                    ContentTypes = { "application/json" }
                };
            }
        }

        [HttpGet]
        public IActionResult getAccount(int accountNumber)
        {
            var temp = DBManager.getByAccountNumber(accountNumber);
            if (temp == null)
            {
                return NotFound(new { Message = "Account not found" });
            }
            else
            {
                return new ObjectResult(accountNumber) { StatusCode = 200 };
            }
        }

        [HttpPut]
        public IActionResult updateAccount(int accountNumber, [FromBody] Account tempAccount)
        {
            tempAccount = (Account)getAccount(accountNumber);
            if (tempAccount == null)
            {
                return NotFound(new { Message = "Account not found" });
            }
            else
            {
                DBManager.updateAccount(tempAccount);
                var response = new { Message = "Account update successfully" };
                return new ObjectResult(response)
                {
                    StatusCode = 200,
                    ContentTypes = { "application/json" }
                };
            }
        }

        [HttpDelete]
        public IActionResult deleteAccount(int accountNumber)
        {
            var temp = getAccount(accountNumber);
            if (temp == null)
            {
                return NotFound(new { Message = "Account not found" });
            }
            else
            {
                DBManager.deleteAccount((Account)temp);
                var response = new { Message = "Account delete successfully" };
                return new ObjectResult(response)
                {
                    StatusCode = 200,
                    ContentTypes = { "application/json" }
                };
            }
        }
    }
}