using FinanceTrackerApi.Entities;
using FinanceTrackerApi.Entities.Login;
using FinanceTrackerApi.Services.LoginOperations;
using MediatR;

namespace FinanceTrackerApi.CommandQueries.Signups.Commands;

public class SignupCommandRequest : IRequest<SignupCommandResponse>
{
    public string FullName{ get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } = string.Empty;
    public SignupCommandRequest(string fullName, string username, string email, string password)
    {
        FullName = fullName;
        Username = username;
        Email = email;
        Password = password;
    }
}
public class SignupCommandResponse
{
    public string username { get; set; }
}

public class SignupCommandRequestHandler(IEncryptionUtility encryptionUtility, AccountDbContext accountDbContext) 
    : IRequestHandler<SignupCommandRequest, SignupCommandResponse>
{
    public async Task<SignupCommandResponse> Handle(SignupCommandRequest request, CancellationToken cancellationToken)
    {
        // Implement signup logic here (e.g., save to database)
        var user = new Users
        {
            FullName = request.FullName,    
            UserName = request.Username,
            Email = request.Email,
            PasswordHash = encryptionUtility.HashPassword(request.Password)
        };

        accountDbContext.Users.Add(user);
        await accountDbContext.SaveChangesAsync(cancellationToken);

        var SignupCommandResponse = new SignupCommandResponse
        {
            username = request.Username
        };

        return SignupCommandResponse;

        // For demonstration, we will just return the username
        //return await Task.FromResult(new SignupCommandResponse
        //{
        //    username = request.Username
        //});
    }
}
