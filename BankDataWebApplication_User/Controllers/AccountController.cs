using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BankDataWebService.Models;
using BankDataWebService.Data;

namespace BankDataWebApplication_User.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/UserAccountSummary/UserAccountSummaryPage.cshtml");
        }

        [HttpGet("summary/{username}")]
        public IActionResult GetAccountSummary(string username)
        {
            Account temp = null;
            User user = DBManager.getByUserName(username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var accounts = DBManager.getAllAcounts();

            foreach (var account in accounts)
            {
                if(account.holderName == user.userName)
                {
                    temp = account;
                }
            }

            return Json(temp);
        }
    }
}