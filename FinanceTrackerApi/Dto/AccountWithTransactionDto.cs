namespace FinanceTrackerApi.Dto
{

    public class AccountWithTransactionDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
    }
}
