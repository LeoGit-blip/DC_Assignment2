using BankDataWebService.Data;
using BankDataWebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankDataWebApplication_Admin.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
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
                    return PartialView("~/Views/AdminLogin/AuthenticatedAdminLoginPage.cshtml");
                }
            }
            return PartialView("~/Views/AdminLogin/AdminLoginPage.cshtml");
        }

        [HttpGet("error")]
        public IActionResult GetLoginErrorView()
        {
            return PartialView("~/Views/AdminLogin/ErrorInAdminLoginPage.cshtml");
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] User user)
        {
            // Return the partial view as HTML
            var response = new { login = false};

            var userlist = DBManager.getByUserName(user.userName);

            if (user!=null && user.userName.Equals(userlist.userName) && user.password.Equals(userlist.password))
            {
                Response.Cookies.Append("SessionID", "1234567");
                response = new { login = true };
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
                    return PartialView("~/Views/AdminLogin/AuthenticatedAdminLoginPage.cshtml");
                }

            }
            // Return the partial view as HTML
            return PartialView("~/Views/AdminLogin/ErrorInAdminLoginPage.cshtml");
        }
    }
}
