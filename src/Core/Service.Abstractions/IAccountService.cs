using Contracts.Accounts;

namespace Service.Abstractions
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountResponse>> GetAccountsByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<AccountResponse> GetAccountByIdAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default);
        Task<AccountResponse> CreateAccountAsync(Guid ownerId, AccountForCreationRequest accountCreationRequest, CancellationToken cancellationToken = default);
        Task DeleteAccountAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default);
    }
}
