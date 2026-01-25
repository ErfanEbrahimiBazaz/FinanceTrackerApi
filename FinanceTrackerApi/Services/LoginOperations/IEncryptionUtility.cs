namespace FinanceTrackerApi.Services.LoginOperations;

public interface IEncryptionUtility
{
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string storedHash);
}