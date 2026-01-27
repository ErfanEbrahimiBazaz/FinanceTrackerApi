using FinanceTrackerApi.Entities.Login;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApi.Entities
{
    public class AccountDbContext: DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationPermission> ApplicationPermissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options)
        {
                
        }
    }
}
