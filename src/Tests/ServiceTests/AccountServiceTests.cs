using Contracts.Accounts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Moq;
using Service;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class AccountServiceTests
    {
        private readonly Mock<IManagerRepository> _managerRepositoryMock;
        private readonly IAccountService _accountService;
        private Guid ownerId = Guid.NewGuid();
        public AccountServiceTests()
        {
            _managerRepositoryMock = new Mock<IManagerRepository>();
            _accountService = new AccountService(_managerRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateAccountAsync_CreateAccount_ShouldReturnsAccountResponseType()
        {
            _managerRepositoryMock.Setup(repo => repo.OwnerRepository.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync(new Owner());
            var account = AccountForCreationObject().Adapt<Account>();
            _managerRepositoryMock.Setup(service => service.AccountRepository.CreateAccount(account));
            _managerRepositoryMock.Setup(service => service.UnitOfWork.SaveChangesAsync(CancellationToken.None));
            var response = await ConfigureAccountService(_managerRepositoryMock).CreateAccountAsync(ownerId, AccountForCreationObject(), CancellationToken.None);
            Assert.IsType<AccountResponse>(response);
        }

        [Fact]
        public async Task CreateAccountAsync_OwnerIdDoesNotExistsInDatabase_ThrowsOwnerNotFoundException()
        {
            _managerRepositoryMock.Setup(repo => repo.OwnerRepository.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync((Owner)null);
            await Assert.ThrowsAsync<OwnerNotFoundException>(() => ConfigureAccountService(_managerRepositoryMock).CreateAccountAsync(ownerId, AccountForCreationObject(), CancellationToken.None));
        }

        [Fact]
        public async Task DeleteAccountAsync_OwnerIdDoesNotExistsInDatabase_ThrowsOwnerNotFoundException()
        {
            var accountId = Guid.NewGuid();
            _managerRepositoryMock.Setup(repo => repo.OwnerRepository.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync((Owner)null);
            await Assert.ThrowsAsync<OwnerNotFoundException>(() => ConfigureAccountService(_managerRepositoryMock).DeleteAccountAsync(ownerId, accountId, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteAccountAsync_AccountIdDoesNotExistsInDatabase_ThrowsAccountNotFoundException()
        {
            var accountId = Guid.NewGuid();
            _managerRepositoryMock.Setup(repo => repo.OwnerRepository.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync(new Owner());
            _managerRepositoryMock.Setup(repo => repo.AccountRepository.GetAccountByIdAsync(accountId, CancellationToken.None)).ReturnsAsync((Account)null);
            await Assert.ThrowsAsync<AccountNotFoundException>(() => ConfigureAccountService(_managerRepositoryMock).DeleteAccountAsync(ownerId, accountId, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteAccountAsync_AccountNotBelongsToOwnerIdInformed_ThrowsAccountDoesNotBelongToOwnerException()
        {
            var accountId = Guid.NewGuid();
            _managerRepositoryMock.Setup(repo => repo.OwnerRepository.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync(new Owner { Id = ownerId });
            _managerRepositoryMock.Setup(repo => repo.AccountRepository.GetAccountByIdAsync(accountId, CancellationToken.None)).ReturnsAsync(new Account { OwnerId = Guid.NewGuid() });
            await Assert.ThrowsAsync<AccountDoesNotBelongToOwnerException>(() => ConfigureAccountService(_managerRepositoryMock).DeleteAccountAsync(ownerId, accountId, CancellationToken.None));
        }

        [Fact]
        public async Task GetAccountByIdAsync_OwnerIdDoesNotExistsInDatabase_ThrowsOwnerDoesNotFoundException()
        {
            _managerRepositoryMock.Setup(repo => repo.OwnerRepository.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync((Owner)null);
            await Assert.ThrowsAsync<OwnerNotFoundException>(() => ConfigureAccountService(_managerRepositoryMock).GetAccountByIdAsync(Guid.NewGuid(), ownerId, CancellationToken.None));
        }

        [Fact]
        public async Task GetAccountByIdAsync_AccountIdDoesNotExistsInDatabase_ThrowsAccountDoesNotFoundException()
        {
            _managerRepositoryMock.Setup(repo => repo.OwnerRepository.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync(new Owner());
            _managerRepositoryMock.Setup(repo => repo.AccountRepository.GetAccountByIdAsync(Guid.NewGuid(), CancellationToken.None)).ReturnsAsync((Account)null);
            await Assert.ThrowsAsync<AccountNotFoundException>(() => ConfigureAccountService(_managerRepositoryMock).GetAccountByIdAsync(ownerId, Guid.NewGuid(), CancellationToken.None));
        }

        [Fact]
        public async Task GetAccountByIdAsync_AccountNotBelongsToOwnerIdInformed_ThrowsAccountDoesNotBelongToOwnerException()
        {
            var accountId = Guid.NewGuid();
            _managerRepositoryMock.Setup(repo => repo.OwnerRepository.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync(new Owner { Id = ownerId });
            _managerRepositoryMock.Setup(repo => repo.AccountRepository.GetAccountByIdAsync(accountId, CancellationToken.None)).ReturnsAsync(new Account { OwnerId = Guid.NewGuid() });
            await Assert.ThrowsAsync<AccountDoesNotBelongToOwnerException>(() => ConfigureAccountService(_managerRepositoryMock).GetAccountByIdAsync(ownerId, accountId, CancellationToken.None));
        }

        [Fact]
        public async Task GetAccountByIdAsync_GetAccountById_ReturnsNotNullAccountResponseType()
        {
            var accountId = Guid.NewGuid();
            _managerRepositoryMock.Setup(repo => repo.OwnerRepository.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync(new Owner { Id = ownerId });
            _managerRepositoryMock.Setup(repo => repo.AccountRepository.GetAccountByIdAsync(accountId, CancellationToken.None)).ReturnsAsync(new Account { OwnerId = ownerId });
            var response = await ConfigureAccountService(_managerRepositoryMock).GetAccountByIdAsync(ownerId, accountId, CancellationToken.None);
            Assert.IsType<AccountResponse>(response);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetAccountsByOwnerAsync_OwnerIdHasAccountsInDatabase_ReturnsIEnumerableAccountResponse()
        {
            _managerRepositoryMock.Setup(repo => repo.AccountRepository.GetAccountsByOwnerIdAsync(Guid.NewGuid(), CancellationToken.None)); 
            var result = await _accountService.GetAccountsByOwnerAsync(Guid.NewGuid(), CancellationToken.None);
            Assert.IsAssignableFrom<IEnumerable<AccountResponse>>(result);
        }

        #region "Methods"
        private static AccountForCreationRequest AccountForCreationObject()
        {
            return new AccountForCreationRequest
            {
                DateCreated = DateTime.UtcNow,
                AccountType = Faker.Name.First()
            };
        }

        private static AccountService ConfigureAccountService(Mock<IManagerRepository> ownerMock)
        {
            return new AccountService(ownerMock.Object);
        }
        #endregion
    }
}
