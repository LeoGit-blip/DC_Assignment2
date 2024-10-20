using BankDataWebApplication_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace BankDataWebApplication_Admin.Controllers
{
    [Route("api/logs")]
    public class LogController : Controller
    {
        private static List<LogEntry> _logs = new List<LogEntry>();

        [HttpGet]
        public IActionResult GetView()
        {
            return PartialView("~/Views/LogsAndAuditTrails/LogsAndAuditTrails.cshtml");
        }

        [HttpGet("fetch")]
        public IActionResult FetchLogs([FromQuery] string username, [FromQuery] string action, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filteredLogs = _logs.AsQueryable();

            if (!string.IsNullOrEmpty(username))
                filteredLogs = filteredLogs.Where(l => l.Username.Contains(username));

            if (!string.IsNullOrEmpty(action))
                filteredLogs = filteredLogs.Where(l => l.Action.Contains(action));

            if (fromDate.HasValue)
                filteredLogs = filteredLogs.Where(l => l.Timestamp >= fromDate.Value);

            if (toDate.HasValue)
                filteredLogs = filteredLogs.Where(l => l.Timestamp <= toDate.Value);

            return Json(filteredLogs.OrderByDescending(l => l.Timestamp).ToList());
        }

        [HttpPost("log")]
        public IActionResult LogAction([FromBody] LogEntry logEntry)
        {
            logEntry.Timestamp = DateTime.Now;
            _logs.Add(logEntry);
            return Json(new { success = true, message = "Action logged successfully" });
        }
    }

}
