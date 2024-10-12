namespace BankDataWebService.Models
{
    public class Transaction
    {
        public int transactionID { get; set; }

        public string transactionName { get; set; }

        public double transactionAmount { get; set; }

        public string transactionType { get; set; }

        public DateTime transactionTime { get; set; }
    }
}
