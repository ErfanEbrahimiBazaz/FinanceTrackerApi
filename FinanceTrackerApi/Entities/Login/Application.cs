using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApi.Entities.Login;

public class Application
{

    [Key]
    public int Id { get; set; }
    public string ApplicationName { get; set; } = string.Empty;
}