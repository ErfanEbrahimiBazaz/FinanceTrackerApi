using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceTrackerApi.Dto;
using FinanceTrackerApi.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApi.Repositories;

public class AccountRepository(AccountDbContext context,
                               IMapper mapper) : IAccountRepository
{
    // 1st approach without using ProjectTo and AutoMapper's profile
    public async Task<Accounts?> GetAccountByIdWithTransactionsAsync(int id)
    {
        var account = await context.Accounts.Include(acc => acc.Transactions).FirstOrDefaultAsync(acc => acc.Id == id);
        return account;
    }

    //2nd approach using ProjectTo and AutoMapper's profile
    //This requires changes in IReposiroty
    public async Task<AccountWithTransactionDto?> GetAccountByIdWithTransactionsProjectToAsync(int id)
    {
        var account = await context.Accounts.ProjectTo<AccountWithTransactionDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(acc => acc.Id == id);
        return account;
    }

    public async Task<IEnumerable<Accounts>> GetAllAccountsWithoutTransactionsAsync()
    {
        return await context.Accounts.ToListAsync<Accounts>();
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsWithoutTransactionsProjectToAsync()
    {
        var accountsDto = await context.Accounts.ProjectTo<AccountDto>(mapper.ConfigurationProvider).ToListAsync();
        return accountsDto;
    }

    public async Task<IEnumerable<Accounts>> GetAllAccountsWithTransactionsAsync()
    {
        return await context.Accounts.Include(acc => acc.Transactions).ToListAsync();
    }

    public async Task<IEnumerable<AccountWithTransactionDto>> GetAllAccountsWithTransactionsProjectToAsync()
    {
        return await context.Accounts.ProjectTo<AccountWithTransactionDto>(mapper.ConfigurationProvider).ToListAsync();
    }
    public async Task CreateAccountAsync(Accounts account)
    {
        if(account == null)
        {
            throw new ArgumentNullException(nameof(account), "Account cannot be null.");
        }

        
        context.Accounts.Add(account);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(int accountId)
    {
        //context.Accounts.Remove(new Account { Id = accountId });
        var account = await context.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if(account == null)
        {
            throw new KeyNotFoundException($"Account with ID {accountId} not found.");
        }
        context.Accounts.Remove(account);
        await context.SaveChangesAsync();
    }
}
