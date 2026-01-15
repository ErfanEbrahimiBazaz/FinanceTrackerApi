using AutoMapper;
using FinanceTrackerApi.Dto;
using FinanceTrackerApi.Entities;

namespace FinanceTrackerApi.Mappers
{
    public class AccountProfileMapper : Profile
    {
        public AccountProfileMapper()
        {
            
            CreateMap<Accounts, AccountWithTransactionDto>().ReverseMap();
            CreateMap<Accounts, AccountDto>().ReverseMap();
        }
    }
}

