using FinanceTrackerApi.Dto;
using FinanceTrackerApi.Entities;

namespace FinanceTrackerApi.Repositories
{
    public interface IAccountRepository
    {

        // 1st approach without using ProjectTo and AutoMapper's profile
        Task<Accounts?> GetAccountByIdWithTransactionsAsync(int id);
        // 2nd approach using ProjectTo and AutoMapper's profile: This requires changes in IReposiroty
        Task<AccountWithTransactionDto?> GetAccountByIdWithTransactionsProjectToAsync(int id);
        Task<IEnumerable<Accounts>> GetAllAccountsWithTransactionsAsync();
        Task<IEnumerable<AccountWithTransactionDto>> GetAllAccountsWithTransactionsProjectToAsync();
        Task<IEnumerable<Accounts>> GetAllAccountsWithoutTransactionsAsync();
        Task<IEnumerable<AccountDto>> GetAllAccountsWithoutTransactionsProjectToAsync();
        Task CreateAccountAsync(Accounts account);
        Task DeleteAccountAsync(int accountId);
    }
}
