using Contracts.Accounts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;

namespace Service
{
    public sealed class AccountService : IAccountService
    {
        private readonly IManagerRepository _managerRepository;
        public AccountService(IManagerRepository managerRepository) => _managerRepository = managerRepository;

        public async Task<AccountResponse> CreateAccountAsync(Guid ownerId, AccountForCreationRequest accountCreationRequest, CancellationToken cancellationToken = default)
        {
            var owner = await _managerRepository.OwnerRepository.GetOwnerByIdAsync(ownerId, cancellationToken);
            if (owner is null)
                throw new OwnerNotFoundException(ownerId);
            var account = accountCreationRequest.Adapt<Account>();
            account.OwnerId = owner.Id;
            _managerRepository.AccountRepository.CreateAccount(account);
            await _managerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return account.Adapt<AccountResponse>();
        }

        public async Task DeleteAccountAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default)
        {
            var owner = await _managerRepository.OwnerRepository.GetOwnerByIdAsync(ownerId, cancellationToken);
            if (owner is null)
                throw new OwnerNotFoundException(ownerId);
            var account = await _managerRepository.AccountRepository.GetAccountByIdAsync(accountId, cancellationToken);
            if (account is null)
                throw new AccountNotFoundException(accountId);
            if (owner.Id != account.OwnerId)
                throw new AccountDoesNotBelongToOwnerException(ownerId, accountId);
            _managerRepository.AccountRepository.DeleteAccount(account);
            await _managerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<AccountResponse> GetAccountByIdAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default)
        {
            var owner = await _managerRepository.OwnerRepository.GetOwnerByIdAsync(ownerId, cancellationToken);
            if (owner is null)
                throw new OwnerNotFoundException(ownerId);
            var account = await _managerRepository.AccountRepository.GetAccountByIdAsync(accountId, cancellationToken);
            if (account is null)
                throw new AccountNotFoundException(accountId);
            if (account.OwnerId != owner.Id)
                throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
            return account.Adapt<AccountResponse>();
        }

        public async Task<IEnumerable<AccountResponse>> GetAccountsByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var accounts = await _managerRepository.AccountRepository.GetAccountsByOwnerIdAsync(ownerId, cancellationToken);
            return accounts.Adapt<IEnumerable<AccountResponse>>();
        }
    }
}
