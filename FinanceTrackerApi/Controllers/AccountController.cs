using AutoMapper;
using FinanceTrackerApi.Dto;
using FinanceTrackerApi.Entities;
using FinanceTrackerApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations; // ModelState is here

namespace FinanceTrackerApi.Controllers
{
    [ApiController]
    [Route("/api/Account")]
    public class AccountController(IAccountRepository repository,
        IMapper mapper) : ControllerBase
    {
        //[HttpGet("AllAccounts")]
        //public ActionResult<List<AccountWithTransactionDto>> GetAllAccountsWithTransactions()
        //{
        //    var accounts = InMemoryDB.Instance.Accounts;
        //    return Ok(accounts);
        //}


        [HttpGet("AllAccounts")]
        public async Task<ActionResult<List<AccountWithTransactionDto>>> GetAllAccountsWithTransactions()
        {
            var accounts = await repository.GetAllAccountsWithTransactionsAsync();
            var accountsDto = mapper.Map<IEnumerable<AccountWithTransactionDto>>(accounts);
            return Ok(accountsDto);
        }

        [HttpGet("AllAccountsWithProjectTo")]
        public async Task<ActionResult<List<AccountWithTransactionDto>>> GetAllAccountsWithTransactionsProjectTo()
        {
            var accounts = await repository.GetAllAccountsWithTransactionsProjectToAsync();
            return Ok(accounts);
        }

        //[HttpGet("AllAccountsWitoutTransactions")]
        //public ActionResult<List<AccountDto>> GetAllAccountsWithoutTransactions()
        //{
        //    var accounts = InMemoryDB.Instance.Accounts;
        //    var accountDtos = new List<AccountDto>();
        //    foreach (var account in accounts)
        //    {
        //        var accountWithoutTransactions = new AccountDto
        //        {
        //            Id = account.Id,
        //            AccountNumber = account.AccountNumber,
        //            Balance = account.Balance,
        //            CreatedAt = account.CreatedAt
        //        };
        //        accountDtos.Add(accountWithoutTransactions);
        //    }
        //    return Ok(accountDtos);
        //}

        [HttpGet("AllAccountsWitoutTransactions")]
        public async Task<ActionResult<List<AccountDto>>> GetAllAccountsWithoutTransactions()
        {
            var accounts = await repository.GetAllAccountsWithoutTransactionsAsync();
            var accountDtos = mapper.Map<IEnumerable<AccountDto>>(accounts);
            return Ok(accountDtos);
        }


        [HttpGet("AllAccountsWitoutTransactionsWithProjectTo")]
        public async Task<ActionResult<List<AccountDto>>> GetAllAccountsWithoutTransactionsProjectTo()
        {
            var accounts = await repository.GetAllAccountsWithoutTransactionsProjectToAsync();
            var accountDtos = mapper.Map<IEnumerable<AccountDto>>(accounts);
            return Ok(accountDtos);
        }

        //[HttpGet("{id}", Name = "GetAccountById")]
        //public ActionResult<AccountWithTransactionDto> GetAccountById(int id)
        //{
        //    var account = InMemoryDB.Instance.Accounts.FirstOrDefault(acc => acc.Id == id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(account);
        //}

        [HttpGet("{id}", Name = "GetAccountById")]
        public async Task<ActionResult<AccountWithTransactionDto>> GetAccountById(int id)
        {
            var account = await repository.GetAccountByIdWithTransactionsAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            var accountDto = mapper.Map<AccountWithTransactionDto>(account);
            return Ok(accountDto);
        }

        [HttpGet("badnaming/{id}", Name = "GetAccountByIdProjectTo")]
        public async Task<ActionResult<AccountWithTransactionDto>> GetAccountByIdProjectTo(int id)
        {
            var account = await repository.GetAccountByIdWithTransactionsProjectToAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        //[HttpPost]
        //public IActionResult CreateAccount([FromBody] AccountsForCreateDto acc)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    int maxId = InMemoryDB.Instance.Accounts.Any() ? InMemoryDB.Instance.Accounts.Max(a => a.Id) : 0;
        //    var account = new AccountWithTransactionDto
        //    {
        //        Id = maxId + 1,
        //        AccountNumber = acc.AccountNumber,
        //        //CreatedAt = acc.CreatedAt,
        //        Balance = acc.Balance,
        //        CreatedAt = DateTime.UtcNow,
        //        Transactions = new List<TransactionDto>()
        //    };
        //    InMemoryDB.Instance.Accounts.Add(account);
        //    return CreatedAtRoute(nameof(GetAccountById), new { id = account.Id }, account);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountsForCreateDto acc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // where http context is not available (out of controllers), in jobs, background services, message consumers, console apps,
            // use validatior:
            Validator.ValidateObject(
                acc,
                new ValidationContext(acc),
                validateAllProperties: true
            );

            var accountEntity = mapper.Map<Accounts>(acc);
            await repository.CreateAccountAsync(accountEntity);
            return CreatedAtRoute(nameof(GetAccountById), new { id = accountEntity.Id }, mapper.Map<AccountWithTransactionDto>(accountEntity));
        }

        [HttpPatch("{accountId}")]
        public IActionResult EditAccountNumber(int accountId, JsonPatchDocument<AccountForPatchDto> patchAccount)
        {
            var accountFromStore = InMemoryDB.Instance.Accounts.FirstOrDefault(acc => acc.Id == accountId);
            if (accountFromStore == null)
            {
                return NotFound();
            }

            var accountToPatch = new AccountForPatchDto()
            {
                Id = accountId,
                AccountNumber = accountFromStore.AccountNumber
            };

            patchAccount.ApplyTo(accountToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (InMemoryDB.Instance.Accounts.Any(acc => acc.AccountNumber == accountToPatch.AccountNumber && acc.Id != accountId))
            {
                return Conflict("Account number already exists.");
            }

            accountFromStore.AccountNumber = accountToPatch.AccountNumber;

            if (!TryValidateModel(accountToPatch))
            {
                return BadRequest(ModelState);
            }

            return NoContent();

        }


    }
}
