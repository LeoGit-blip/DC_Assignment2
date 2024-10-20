using BankDataWebService.Data;
using BankDataWebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankDataWebApplication_Admin.Controllers
{
    [Route("api/adminprofile")]
    public class AdminProfileController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            return PartialView("~/Views/AdminProfileInformation/AdminProfileInformationPage.cshtml");
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


        [HttpPut("update/{oldUserName}")]
        public IActionResult UpdateProfile(string oldUserName, [FromBody] User user)
        {
            if (string.IsNullOrEmpty(oldUserName))
            {
                return BadRequest("Old username is required");
            }
            var existingUser = DBManager.getByUserName(oldUserName);
            if (existingUser == null)
            {
                return NotFound($"User with username {oldUserName} not found");
            }

            // Update user properties
            existingUser.userName = user.userName ?? existingUser.userName;
            existingUser.email = user.email ?? existingUser.email;
            existingUser.phone = user.phone;
            existingUser.address = user.address ?? existingUser.address;
            existingUser.password = user.password ?? existingUser.password;

            if (DBManager.updateUser(existingUser, oldUserName))
            {
                var response = new { Message = "Account updated successfully" };
                return Ok(response);
            }
            return BadRequest("Could not update. The new username may already be in use.");
        }
    }
}
