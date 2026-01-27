using FinanceTrackerApi.Entities;
using FinanceTrackerApi.Services.LoginOperations;
using FinanceTrackerApi.Services.TokenService;
using MediatR;

namespace FinanceTrackerApi.CommandQueries.Logins.Commands;

public class LoginCommandRequest: IRequest<LoginCommandResponse>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginCommandResponse
{
    public string Token { get; set; }
}

public class LoginCommandHandler(IEncryptionUtility encryptionUtility,
    AccountDbContext dbContext,
    IJwtService jwtService) : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
{
    public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        var user = dbContext.Users.SingleOrDefault(u => u.UserName == request.UserName);
        if(user == null)
        {
            throw new Exception("Invalid username or password.");
        }
        string passwordHash = user.PasswordHash;
        var result = encryptionUtility.VerifyPassword(request.Password, passwordHash);
        
        if(!result)
        {
            throw new Exception("Invalid username or password.");
        }
        // For demonstration, we will just return a dummy token
        return await Task.FromResult(new LoginCommandResponse
        {
            Token = jwtService.GenerateAccessToken(user)
        });
    }
}
