using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerApi.Entities.Login;

public class RefreshToken
{
    [Key]
    public long Id { get; set; }
    [ForeignKey(nameof(UserName))]
    public string UserName { get; set; }
    public virtual User? User { get; set; }
    public Guid RefreshTokenValue { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  
    public DateTime ExpiresAt { get; set; }

}
