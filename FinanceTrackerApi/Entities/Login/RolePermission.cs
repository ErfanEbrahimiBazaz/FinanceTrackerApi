using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerApi.Entities.Login;

public class RolePermission
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Application")]
    public int ApplicationId { get; set; }
    public virtual Application Application{ get; set; }
    [ForeignKey(nameof(Login.Role))]
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
}
