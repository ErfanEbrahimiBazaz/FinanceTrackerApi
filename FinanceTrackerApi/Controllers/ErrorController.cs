using Microsoft.AspNetCore.Mvc;

namespace FinanceTrackerApi.Controllers;

[ApiController]
[Route("error")]
public class ErrorController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("/error")]
    public IActionResult Error()
    {
        return Problem(
            title: "An unexpected error occurred",
            statusCode: 500
        );
    }
}
