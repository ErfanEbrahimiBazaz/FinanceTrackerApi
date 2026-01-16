using AutoMapper;
using FinanceTrackerApi.Dto;
using FinanceTrackerApi.Entities;

namespace FinanceTrackerApi.Mappers;

public class AccountProfileMapper : Profile
{
    public AccountProfileMapper()
    {
        CreateMap<AccountsForCreateDto, Accounts>();
        CreateMap<Accounts, AccountWithTransactionDto>();

        // Entity -> Dto OK
        // Dto -> Entity NOT OK
        CreateMap<Accounts, AccountDto>(); // “Stripe rule: Never allow mapping INTO aggregates”, Entity -> Dto 
        CreateMap<Transaction, TransactionDto>();
    }
}

