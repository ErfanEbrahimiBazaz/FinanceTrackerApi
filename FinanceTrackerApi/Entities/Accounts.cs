using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerApi.Entities
{
    public class Accounts
    {
        // No setters exposed
        // No invalid state allowed

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [MaxLength(20)]
        public string AccountNumber { get; private set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Balance { get; private set; }
        public IEnumerable<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public Accounts(string accountNumber, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new ArgumentException("Account number cannot be null or empty.", nameof(AccountNumber));
            }
            if (balance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Balance), "Balance cannot be negative.");
            }

            AccountNumber = accountNumber;
            Balance = balance;
        }

        // Method to update account number
        public void UpdateAccountNumber(string newAccountNumber)
        {
            if (string.IsNullOrWhiteSpace(newAccountNumber))
            {
                throw new ArgumentException("Account number cannot be null or empty.", nameof(newAccountNumber));
            }
            AccountNumber = newAccountNumber;
        }

        public decimal Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be positive.");
            }
            Balance += amount;
            return Balance;
        }
        
        public decimal Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Withdrawal amount must be positive.");
            }
            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient balance.");
            }
            Balance -= amount;
            return Balance;
        }
    }
}