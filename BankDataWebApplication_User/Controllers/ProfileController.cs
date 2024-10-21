using BankDataWebService.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankDataWebApplication_User.Controllers
{
    [Route("api/profile")]
    public class ProfileController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/UserProfileInformation/UserProfileInformationPage.cshtml");
        }

        [HttpGet("username/{userName}")]
        public IActionResult getUserByName(string userName)
        {
            var temp = DBManager.getByUserName(userName);

            if (temp == null)
            {
                return NotFound(new { Message = "User name not found" });
            }
            else
            {
                return Ok(temp);
            }
        }
    }
}
