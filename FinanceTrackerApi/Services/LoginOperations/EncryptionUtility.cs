using System.Security.Cryptography;
namespace FinanceTrackerApi.Services.LoginOperations;

public class EncryptionUtility: IEncryptionUtility
{
    // The salt size in bytes (128 bits)
    private const int SaltSize = 16;
    // The hash size in bytes (256 bits)
    private const int HashSize = 32;
    // The number of iterations (OWASP recommends at least 100,000)
    private const int Iterations = 100000;

    public string HashPassword(string password)
    {
        // Generate a cryptographically strong random salt
        byte[] salt;
        using (var rng = RandomNumberGenerator.Create())
        {
            salt = new byte[SaltSize];
            rng.GetBytes(salt);
        }

        // Derive a key (hash) from the password and salt using PBKDF2
        byte[] hash;
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        hash = pbkdf2.GetBytes(HashSize);

        // Combine the salt and hash into a single string for storage (salt first)
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, HashSize);

        // Convert the byte array to a Base64 string for easy storage in a database
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        // Extract the salt and hash from the stored string
        byte[] hashBytes = Convert.FromBase64String(storedHash);
        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        // Compute the hash on the provided password using the extracted salt
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] inputHash = pbkdf2.GetBytes(HashSize);

        // Compare the computed hash with the stored hash
        // Use a fixed-time comparison to mitigate timing attacks
        //return CryptographicOperations.FixedTimeEquals(inputHash, hashBytes, SaltSize);
        return CryptographicOperations.FixedTimeEquals(inputHash, hashBytes.AsSpan(SaltSize, HashSize));
    }

    // cryptographiclly insecure, the right version is above
    //public string GeneratePasswordHash(string password, string passwordSalt)
    //{
    //    string saltedPassword = string.Concat(password, passwordSalt);
    //    var passwordHash = Convert.ToBase64String(
    //        SHA256.HashData(
    //            System.Text.Encoding.UTF8.GetBytes(saltedPassword)
    //        )
    //    );
    //    return passwordHash;
    //}

    //// Never return salt as a separate method.
    //public Guid GeneratePasswordSalt()
    //{
    //    var passwordSalt = Guid.NewGuid();
    //    return passwordSalt;
    //}
}
