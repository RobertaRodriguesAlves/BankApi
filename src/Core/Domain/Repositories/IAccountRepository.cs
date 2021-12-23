using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccountsByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<Account> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
        void CreateAccount(Account account);
        void DeleteAccount(Account account);
    }
}
