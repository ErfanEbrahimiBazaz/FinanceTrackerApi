using FinanceTrackerApi.Entities;

namespace FinanceTrackerApi.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByIdWithTransactionsAsync(int id);
        Task<IEnumerable<Account>> GetAllAccountsWithTransactionsAsync();
        Task<IEnumerable<Account>> GetAllAccountsWithoutTransactionsAsync();
        Task CreateAccountAsync();
        Task DeleteAccountAsync();
    }
}
