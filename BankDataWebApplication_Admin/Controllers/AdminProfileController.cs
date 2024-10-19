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
    }
}
