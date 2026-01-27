using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerApi.Entities.Login;

public class UserRole
{
    public int Id { get; set; }
    [ForeignKey("User")]
    public string UserName { get; set; }
    public virtual User User { get; set; }
    [ForeignKey(nameof(Login.Role))]
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
}
