using AutoMapper;

namespace FinanceTrackerApi.Mappers
{
    public class AccountProfileMapper : Profile
    {
        public AccountProfileMapper()
        {
            CreateMap<Dto.AccountsForCreateDto, Dto.AccountWithTransactionDto>().ReverseMap();
            CreateMap<Dto.AccountForPatchDto, Dto.AccountWithTransactionDto>();
        }
    }
}

