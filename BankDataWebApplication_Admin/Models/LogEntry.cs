namespace BankDataWebApplication_Admin.Models
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }
    }
}
