using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerApi.Entities
{
    public class Accounts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotNull]
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}