using FinanceTrackerApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApi.Repositories
{
    public class AccountRepository(AccountDbContext context) : IAccountRepository
    {
        public async Task<Account> GetAccountByIdWithTransactionsAsync(int id)
        {
            var account = await context.Accounts.Include(acc => acc.Transactions).FirstOrDefaultAsync(acc => acc.Id == id);
            if(account == null)
            {
                throw new KeyNotFoundException($"Account with ID {id} not found.");
            }
            return account;
        }
        public async Task<IEnumerable<Account>> GetAllAccountsWithoutTransactionsAsync()
        {
            return await context.Accounts.ToListAsync<Account>();
        }
        public async Task<IEnumerable<Account>> GetAllAccountsWithTransactionsAsync()
        {
            return await context.Accounts.Include(acc => acc.Transactions).ToListAsync();
        }
        public async Task CreateAccountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAccountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
