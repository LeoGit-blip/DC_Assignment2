using BankDataWebApplication_User.Models;
using BankDataWebService.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankDataWebApplication_User.Controllers
{
    [Route("api/editprofile")]
    public class EditUserController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/EditUserInformation/EditUserInformationPage.cshtml");
        }

        [HttpPost("update")]
        public IActionResult UpdateUserInfo([FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = DBManager.getByUserName(request.OldUsername);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            // Update user information only if new values are provided
            if (request.NewUsername != null)
            {
                existingUser.userName = request.NewUsername;
            }

            if (request.Email != null)
            {
                existingUser.email = request.Email;
            }

            if (request.PhoneNumber.HasValue)
            {
                existingUser.phone = request.PhoneNumber.Value;
            }

            if (request.NewPassword != null)
            {
                existingUser.password = request.NewPassword;
            }

            // Update user in the database
            bool updateSuccess = DBManager.updateUser(existingUser, request.OldUsername);

            if (updateSuccess)
            {
                return Ok("User information updated successfully");
            }
            else
            {
                return StatusCode(500, "Failed to update user information");
            }
        }
    }
}
