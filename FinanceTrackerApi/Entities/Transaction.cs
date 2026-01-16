using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FinanceTrackerApi.Entities
{
    public class Transaction
    {
        // No setters exposed
        // No invalid state allowed

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public Accounts Account { get; set; }
        [NotNull]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Credit or Debit

    }
}