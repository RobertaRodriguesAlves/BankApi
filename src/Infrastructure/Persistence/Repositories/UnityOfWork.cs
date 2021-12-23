using Domain.Repositories;

namespace Persistence.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly BankDbContext _context;

        public UnitOfWork(BankDbContext context) => _context = context;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _context.SaveChangesAsync(cancellationToken);
    }
}
