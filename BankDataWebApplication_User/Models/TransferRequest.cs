namespace BankDataWebApplication_User.Models
{
    public class TransferRequest
    {
        public string SenderUsername { get; set; }
        public int RecipientAccountNumber { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
