using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FinanceTrackerApi.Entities.Login;

public class ApplicationPermission
{
    [Key]
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    [NotNull]
    public string Permission { get; set; } = string.Empty;
    public string? PermissionDescription { get; set; }
}
