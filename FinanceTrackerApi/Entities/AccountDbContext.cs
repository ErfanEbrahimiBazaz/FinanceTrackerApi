using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApi.Entities
{
    public class AccountDbContext: DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options)
        {
                
        }
    }
}
