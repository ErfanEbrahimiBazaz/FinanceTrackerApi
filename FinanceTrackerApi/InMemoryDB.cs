using FinanceTrackerApi.Dto;

namespace FinanceTrackerApi
{
    public class InMemoryDB
    {
        private static readonly InMemoryDB instance = new InMemoryDB(); //thread-safe singleton
        public static InMemoryDB Instance => instance;
        public List<AccountWithTransactionDto> Accounts { get; set; } = new List<AccountWithTransactionDto>();
        public List<TransactionDto> Transactions { get; set; } = new List<Dto.TransactionDto>();
        private InMemoryDB()   //thread-safe singleton, chand to public if using DI and adding to services.
        {
            //Accounts.Add(
            //new Dto.AccountDto
            //{
            //    Id = 1,
            //    AccountNumber = "1234567890",
            //    Balance = 1000.00m,
            //    CreatedAt = DateTime.UtcNow
            //});

            Accounts.AddRange(new[]
            {
                new AccountWithTransactionDto()
                {
                Id = 1,
                AccountNumber = "1234567890",
                Balance = 1000.00m,
                CreatedAt = DateTime.UtcNow,
                Transactions = new List<TransactionDto>()
                {
                    new TransactionDto{
                    Id = 1,
                    AccountId = 1,
                    Amount = 200.00m,
                    Description = "Grocery Shopping",
                    Timestamp = DateTime.UtcNow,
                    Type = "Debit"
                    },
                    new TransactionDto{
                    Id = 2,
                    AccountId = 1,
                    Amount = -100.00m,
                    Description = "Grocery Shopping",
                    Timestamp = DateTime.UtcNow,
                    Type = "Credit"
                    },
                },
            },


            new AccountWithTransactionDto()
                {
                Id = 2,
                AccountNumber = "2134567890",
                Balance = 2000.00m,
                CreatedAt = DateTime.UtcNow,
                Transactions = new List<TransactionDto>()
                {
                    new TransactionDto{
                    Id = 3,
                    AccountId = 2,
                    Amount = 200.00m,
                    Description = "7-eleven Shopping",
                    Timestamp = DateTime.UtcNow,
                    Type = "Debit"
                    },
                    new TransactionDto{
                    Id = 4,
                    AccountId = 2,
                    Amount = -100.00m,
                    Description = "Credit",
                    Timestamp = DateTime.UtcNow,
                    Type = "Credit"
                    },
                },
            },

            new AccountWithTransactionDto()
            {
                Id = 3,
                AccountNumber = "3214567890",
                Balance = 500.00m,
                CreatedAt = DateTime.UtcNow,
                Transactions = new List<TransactionDto>()
                {
                    new TransactionDto{
                    Id = 5,
                    AccountId = 3,
                    Amount = 200.00m,
                    Description = "7-eleven Shopping",
                    Timestamp = DateTime.UtcNow,
                    Type = "Debit"
                    },
                    new TransactionDto{
                    Id = 6,
                    AccountId = 3,
                    Amount = -100.00m,
                    Description = "Credit",
                    Timestamp = DateTime.UtcNow,
                    Type = "Credit"
                    },
                },
            },


            });
        }
    }
}
