using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BankDataWebService.Models;
using BankDataWebService.Data;

namespace BankDataWebApplication_Admin.Controllers
{
    [Route("api/usermanagement")]
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/UserManagement/UserManagementPage.cshtml");
        }
        [HttpGet("search")]
        public IActionResult SearchUsers([FromQuery] string criteria, [FromQuery] string value)
        {
            List<User> users;
            if (criteria.ToLower() == "name")
            {
                var user = DBManager.getByUserName(value);
                users = user != null ? new List<User> { user } : new List<User>();
            }
            else if (criteria.ToLower() == "email")
            {
                var user = DBManager.getByUserEmail(value);
                users = user != null ? new List<User> { user } : new List<User>();
            }
            else
            {
                users = DBManager.getAllUsers();
            }

            return Ok(users);
        }

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }

            bool result = DBManager.insertUser(user);
            if (result)
            {
                return Ok(new { message = "User created successfully" });
            }
            return BadRequest(new { message = "Failed to create user" });
        }

        [HttpPut("edit/{username}")]
        public IActionResult EditUser(string username, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }

            var existingUser = DBManager.getByUserName(username);
            if (existingUser == null)
            {
                return NotFound($"User with username {username} not found");
            }

            bool result = DBManager.updateUser(user, username);
            if (result)
            {
                return Ok(new { message = "User updated successfully" });
            }
            return BadRequest(new { message = "Failed to update user" });
        }

        [HttpPost("deactivate/{username}")]
        public IActionResult DeactivateUser(string username)
        {
            var user = DBManager.getByUserName(username);
            if (user == null)
            {
                return NotFound($"User with username {username} not found");
            }

            bool result = DBManager.deleteUserByName(user);
            if (result)
            {
                return Ok(new { message = "User deactivated successfully" });
            }
            return BadRequest(new { message = "Failed to deactivate user" });
        }

        [HttpPost("reset-password/{username}")]
        public IActionResult ResetPassword(string username)
        {
            var user = DBManager.getByUserName(username);
            if (user == null)
            {
                return NotFound($"User with username {username} not found");
            }

            // Generate a new random password
            string newPassword = DBManager.randomString(10);
            user.password = newPassword;

            bool result = DBManager.updateUser(user, username);
            if (result)
            {
                return Ok(new { message = "Password reset successfully", newPassword });
            }
            return BadRequest(new { message = "Failed to reset password" });
        }

    }
}