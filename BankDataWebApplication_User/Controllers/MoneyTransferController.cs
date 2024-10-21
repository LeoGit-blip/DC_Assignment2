using BankDataWebApplication_User.Models;
using BankDataWebService.Data;
using BankDataWebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankDataWebApplication_User.Controllers
{
    [Route("api/transfer")]
    public class MoneyTransferController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/MoneyTransfer/MoneyTransferPage.cshtml");
        }

        [HttpPost]
        public IActionResult ProcessTransfer([FromBody] TransferRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Find sender's account based on username
                Account senderAccount = null;
                User user = DBManager.getByUserName(request.SenderUsername);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                var accounts = DBManager.getAllAcounts();
                senderAccount = accounts.FirstOrDefault(account => account.holderName == user.userName);

                if (senderAccount == null)
                {
                    return BadRequest("Sender account not found.");
                }

                // Validate recipient's account
                var recipientAccount = DBManager.getByAccountNumber(request.RecipientAccountNumber);
                if (recipientAccount == null)
                {
                    return BadRequest("Recipient account not found.");
                }

                // Check if sender has sufficient balance
                if (senderAccount.balance < request.Amount)
                {
                    return BadRequest("Insufficient funds.");
                }

                // Process the transfer
                senderAccount.balance -= request.Amount;
                recipientAccount.balance += request.Amount;

                // Update accounts in the database
                DBManager.updateAccount(senderAccount);
                DBManager.updateAccount(recipientAccount);

                // Create and save transaction records
                var senderTransaction = new Transaction
                {
                    transactionName = $"Transfer to {recipientAccount.holderName}",
                    transactionAmount = request.Amount,
                    transactionType = "Withdraw",
                    transactionTime = DateTime.Now
                };
                var recipientTransaction = new Transaction
                {
                    transactionName = $"Transfer from {senderAccount.holderName}",
                    transactionAmount = request.Amount,
                    transactionType = "Deposit",
                    transactionTime = DateTime.Now
                };

                DBManager.insertTransaction(senderTransaction);
                DBManager.insertTransaction(recipientTransaction);

                return Ok("Transfer completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in ProcessTransfer: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the transfer.");
            }
        }
    }
}
