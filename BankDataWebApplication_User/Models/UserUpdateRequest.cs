namespace BankDataWebApplication_User.Models
{
    public class UserUpdateRequest
    {
        public string OldUsername { get; set; }
        public string? NewUsername { get; set; }
        public string? Email { get; set; }
        public int? PhoneNumber { get; set; }
        public string? NewPassword { get; set; }
    }
}
