namespace BankDataWebService.Models
{
    public class Account
    {
        public int accountNumber { get; set; }

        public float balance { get; set; }

        public string? holderName { get; set; }

        public int phoneNumber { get; set; }

        public string? email { get; set; }
    }
}
