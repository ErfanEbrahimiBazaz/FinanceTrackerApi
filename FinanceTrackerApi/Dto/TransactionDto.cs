namespace FinanceTrackerApi.Dto
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public AccountDto Account { get; set; } // navigation property
        public decimal Amount { get; set; }

        public string Type { get; set; } = string.Empty; // Credit or Debit
        public string Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
