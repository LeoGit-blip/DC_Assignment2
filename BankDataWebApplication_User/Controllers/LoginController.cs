using BankDataWebService.Data;
using BankDataWebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankDataWebApplication_User.Controllers
{
    [Route("api/user")]
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult GetDefaultView()
        {
            return PartialView("~/Views/Home/Index.cshtml");
        }

        [HttpGet("login")]
        public IActionResult GetLoginPage()
        {
            var userlist = DBManager.getAllUsers();

            foreach (User user in userlist)
            {
                Console.WriteLine(user.userName + " " + user.password);
            }

            Response.Cookies.Delete("SessionID");

            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("~/Views/Login/AuthenticatedUserLoginPage.cshtml");
                }
            }
            return PartialView("~/Views/Login/LoginPage.cshtml");
        }

        [HttpGet("error")]
        public IActionResult GetLoginErrorView()
        {
            return PartialView("~/Views/Login/ErrorInLoginPage.cshtml");
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] User user)
        {
            var response = new { login = false, message = "" };
            var userlist = DBManager.getByUserName(user.userName);

            if (userlist == null)
            {
                response = new { login = false, message = "User not found" };
            }
            else if (user.userName.Equals(userlist.userName) && user.password.Equals(userlist.password))
            {
                Response.Cookies.Append("SessionID", "1234567");
                response = new { login = true, message = "Login successful" };
            }
            else
            {
                response = new { login = false, message = "Invalid username or password" };
            }

            return Json(response);
        }

        [HttpGet("authview")]
        public IActionResult GetLoginAuthenticatedView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("~/Views/Login/AuthenticatedUserLoginPage.cshtml");
                }

            }
            // Return the partial view as HTML
            return PartialView("~/Views/Login/ErrorInLoginPage.cshtm");
        }
    }
}
