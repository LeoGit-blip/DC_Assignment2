using BankDataWebService.Data;
using BankDataWebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("addUser")]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "Detect user is null" });
            }
            if (user.userName == null)
            {
                return BadRequest(new { Message = "Detect userName is null" });
            }
            if (user.email == null)
            {
                return BadRequest(new { Message = "Detect userEmail is null" });
            }
            if (user.address == null)
            {
                return BadRequest(new { Message = "Detect userAddress is null" });
            }
            if (user.phone == null)
            {
                return BadRequest(new { Message = "Detect userPhone is null" });
            }
            else
            {
                bool userInsert = DBManager.insertUser(user);
                if (userInsert)
                {
                    var response = new { Message = "User create successfully" };
                    return new ObjectResult(response);
                }
                else
                {
                    return BadRequest(new { Message = "Cant insert user" });
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

        [HttpGet]
        [Route("email/{email}")]
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

        [HttpGet]
        [Route("username/{UserName}")]
        public IActionResult getUserByName(String UserName)
        {
            var temp = DBManager.getByUserName(UserName);
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
        [Route("update/{UserName}")]
        public IActionResult updateUser(string UserName, [FromBody] User user)
        {
            if (user == null)
            {
                return NotFound(new { Message = "Email not found" });
            }
            if (user.userName == null || user.email == null || user.address == null || user.password == null || user.phone == null)
            {
                return BadRequest(new { Message = "Some fields are required" });
            }
            user.userName = UserName;
            bool userUpdate = DBManager.updateUser(user, UserName);
            if (userUpdate)
            {
                var response = new { Message = "Account update successfully" };
                return Ok(response);
            }
            else
            {
                return BadRequest("Could not update");
            }
        }

        [HttpDelete]
        [Route("delete/email/{email}")]
        public IActionResult deleteUserByEmail(String email)
        {
            var temp = DBManager.getByUserEmail(email);
            if (temp == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            else
            {
                DBManager.deleteUserByEmail(temp);
                var response = new { Message = "User delete successfully" };
                return new ObjectResult(response)
                {
                    StatusCode = 200,
                    ContentTypes = { "application/json" }
                };
            }
        }

        [HttpDelete]
        [Route("delete/UserName/{UserName}")]
        public IActionResult deleteUserByName(String UserName)
        {
            var temp = DBManager.getByUserName(UserName);
            if (DBManager.deleteUserByName(temp))
            {
                var response = new { Message = "User delete successfully" };
                return Ok(response);

            }
            return NotFound(new { Message = "User not found" });
        }
    }
}