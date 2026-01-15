using FinanceTrackerApi.Entities;

namespace FinanceTrackerApi.Repositories
{
    public interface IAccountRepository
    {
        Task<Accounts?> GetAccountByIdWithTransactionsAsync(int id);
        Task<IEnumerable<Accounts>> GetAllAccountsWithTransactionsAsync();
        Task<IEnumerable<Accounts>> GetAllAccountsWithoutTransactionsAsync();
        Task CreateAccountAsync();
        Task DeleteAccountAsync(int accountId);
    }
}
