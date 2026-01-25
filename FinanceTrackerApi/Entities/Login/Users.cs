using System.ComponentModel.DataAnnotations;
namespace FinanceTrackerApi.Entities.Login;

public class Users
{
    public string? FullName { get; set; }
    [Key]
    public string UserName { get; set; }
    public string? Email { get; set; }
    //public string PasswordSalt { get; set; }
    
    // hashed salt is prepended to the hashed password 
    public string PasswordHash{ get; set; } = string.Empty;
}
