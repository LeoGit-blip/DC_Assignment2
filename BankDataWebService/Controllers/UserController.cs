using BankDataWebService.Data;
using BankDataWebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "Detect user is null" });
            }
            else
            {
                DBManager.insertUser(user);
                var response = new { Message = "User create successfully" };
                return new ObjectResult(response)
                {
                    StatusCode = 200,
                    ContentTypes = { "application/json" }
                };
            }
        }

        [HttpGet]
        public IActionResult getAllUser()
        {
            var users = DBManager.getAllUsers();
            if (users == null)
            {
                return NotFound(new { Message = "The list is empty" });
            }
            return Ok(users);
        }

        [HttpGet("email/{email}")]
        public IActionResult getUserByEmail(String email)
        {
            var temp = DBManager.getByUserEmail(email);

            if (temp == null)
            {
                return NotFound(new { Message = "Email not found" });
            }
            else
            {
                return Ok(temp);
            }
        }

        [HttpGet("username/{userName}")]
        public IActionResult getUserByName(String userName)
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

        [HttpPut]
        public IActionResult updateUser(string email, string userName, [FromBody] User user)
        {
            var tempEmail = getUserByEmail(email);
            var tempName = getUserByName(userName);

            if (tempEmail == null)
            {
                return NotFound(new { Message = "Email not found" });
            }
            if (tempName == null)
            {
                return NotFound(new { Message = "User Name not found" });
            }
            else if (DBManager.updateUser(user))
            {
                var response = new { Message = "Account update successfully" };
                return new ObjectResult(response)
                {
                    StatusCode = 200,
                    ContentTypes = { "application/json" }
                };
            }
            return BadRequest("Could not update");
        }

        [HttpDelete]
        public IActionResult deleteUser(User user)
        {
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            else
            {
                DBManager.deleteUser(user);
                var response = new { Message = "User delete successfully" };
                return new ObjectResult(response)
                {
                    StatusCode = 200,
                    ContentTypes = { "application/json" }
                };
            }
        }
    }
}
