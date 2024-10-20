using Microsoft.AspNetCore.Mvc;

namespace BankDataWebApplication_User.Controllers
{
    [Route("api/logout")]
    public class LogOutController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            Response.Cookies.Delete("SessionID");
            return PartialView("~/Views/Logout/LogoutPage.cshtml");
        }
    }
}
