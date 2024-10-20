using Microsoft.AspNetCore.Mvc;

namespace BankDataWebApplication_Admin.Controllers
{
    [Route("api/logs")]
    public class LogController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            return PartialView("~/Views/LogsAndAuditTrails/LogsAndAuditTrails.cshtml");
        }

    }
}
