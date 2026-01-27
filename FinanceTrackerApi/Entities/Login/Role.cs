using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApi.Entities.Login;

public class Role
{
    [Key]
    public int Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
