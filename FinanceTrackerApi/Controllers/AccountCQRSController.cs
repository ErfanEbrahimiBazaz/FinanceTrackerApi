using FinanceTrackerApi.CommandQueries.Logins.Commands;
using FinanceTrackerApi.CommandQueries.Signups.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FinanceTrackerApi.Controllers;

[ApiController]
[Route("api/AccountCQRS")]
public class AccountCQRSController(IMediator mediator): ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<SignupCommandResponse>> CreateAccount([FromBody] SignupCommandRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<LoginCommandResponse>> LoginAccount([FromBody] LoginCommandRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
}
