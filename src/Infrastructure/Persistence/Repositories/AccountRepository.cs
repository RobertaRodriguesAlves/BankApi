using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    internal sealed class AccountRepository : IAccountRepository
    {
        private readonly BankDbContext _context;
        public AccountRepository(BankDbContext context)
        {
            _context = context;
        }
        public void CreateAccount(Account account) => _context.Accounts.Add(account);
        public void DeleteAccount(Account account) => _context.Accounts.Remove(account);
        public async Task<Account> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.FirstOrDefaultAsync(account => account.Id.Equals(accountId), cancellationToken);
        }
        public async Task<IEnumerable<Account>> GetAccountsByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.Where(account => account.OwnerId.Equals(ownerId)).ToListAsync(cancellationToken);
        }
    }
}
