using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public sealed class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Account> Accounts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
            => builder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);
    }
}
