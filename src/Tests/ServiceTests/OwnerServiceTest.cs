using Contracts.Owners;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using Service;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public sealed class OwnerServiceTest
    {
        private Mock<IOwnerRepository> _ownerRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private OwnerService _ownerService;
        private Guid ownerId = Guid.NewGuid();

        public OwnerServiceTest()
        {
            _ownerRepositoryMock = new Mock<IOwnerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _ownerService = ConfigureOwnerService(_ownerRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task CreateOwnerAsync_CreateOwner_ShouldSuccessCallRepositoriesAndReturnsOwnerResponseType()
        {
            var result = await _ownerService.CreateOwnerAsync(OwnerForCreationObject(), CancellationToken.None);
            Assert.IsType<OwnerResponse>(result);
            _ownerRepositoryMock.Verify(repo => repo.CreateOwner(It.IsAny<Owner>()), Times.Once());
            _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task DeleteOwnerAsync_OwnerDoesNotExistsInDatabase_ThrowsOwnerNotFoundException()
        {
            _ownerRepositoryMock.Setup(e => e.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync((Owner)null);
            await Assert.ThrowsAsync<OwnerNotFoundException>(() => ConfigureOwnerService(_ownerRepositoryMock, _unitOfWorkMock).DeleteOwnerAsync(ownerId, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteOwnerAsync_DeleteAnExistsOwner_ShouldSuccessCallRepositories()
        {
            _ownerRepositoryMock.Setup(repo => repo.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync(new Owner());
            await ConfigureOwnerService(_ownerRepositoryMock, _unitOfWorkMock).DeleteOwnerAsync(ownerId, CancellationToken.None);
            _ownerRepositoryMock.Verify(repo => repo.DeleteOwner(It.IsAny<Owner>()), Times.Once());
            _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetOwnerByIdAsync_OwnerExistsInDatabase_RetursOwnerResponseType()
        {
            _ownerRepositoryMock.Setup(repo => repo.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync(new Owner());
            var result = await ConfigureOwnerService(_ownerRepositoryMock, _unitOfWorkMock).GetOwnerByIdAsync(ownerId, CancellationToken.None);
            Assert.IsType<OwnerResponse>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetOwnerByIdAsync_OwnerDoesNotExistsInDatabase_ThrowsOwnerNotFoundException()
        {
            _ownerRepositoryMock.Setup(repo => repo.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync((Owner)null);
            await Assert.ThrowsAsync<OwnerNotFoundException>(() => ConfigureOwnerService(_ownerRepositoryMock, _unitOfWorkMock).GetOwnerByIdAsync(ownerId, CancellationToken.None));
        }

        [Fact]
        public async Task GetOwnersAsync_OwnersExistsInDatabase_ShouldSuccessCallRepository()
        {
            var result = await _ownerService.GetOwnersAsync(CancellationToken.None);
            _ownerRepositoryMock.Verify(repo => repo.GetOwnersAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetOwnersAsync_DatabaseHasOwners_ReturnsIEnumerableOwnerResponse()
        {
            var result = await _ownerService.GetOwnersAsync(CancellationToken.None);
            Assert.IsAssignableFrom<IEnumerable<OwnerResponse>>(result);
        }

        [Fact]
        public async Task UpdateOwnerAsync_OwnerExistsInDatabase_ShouldSuccessCallRepositories()
        {
            _ownerRepositoryMock.Setup(repo => repo.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync(new Owner());
            await ConfigureOwnerService(_ownerRepositoryMock, _unitOfWorkMock).UpdateOwnerAsync(ownerId, OwnerForUpdateObject(), CancellationToken.None);
            _ownerRepositoryMock.Verify(repo => repo.UpdateOwner(It.IsAny<Owner>()), Times.Once);
            _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once);
        }


        [Fact]
        public async Task UpdateOwnerAsync_OwnerDoesNotExistsInDatabase_ThrowsOwnerNotFoundException()
        {
            _ownerRepositoryMock.Setup(repo => repo.GetOwnerByIdAsync(ownerId, CancellationToken.None)).ReturnsAsync((Owner)null);
            await Assert.ThrowsAnyAsync<OwnerNotFoundException>(() => ConfigureOwnerService(_ownerRepositoryMock, _unitOfWorkMock).UpdateOwnerAsync(ownerId, OwnerForUpdateObject(), CancellationToken.None));
        }

        #region "Methods"
        private static OwnerService ConfigureOwnerService(Mock<IOwnerRepository> ownerMock, Mock<IUnitOfWork> unitWorkMock)
        {
            return new OwnerService(ownerMock.Object, unitWorkMock.Object);
        }

        private static OwnerForCreationRequest OwnerForCreationObject()
        {
            return new OwnerForCreationRequest
            {
                Name = Faker.Name.FullName(),
                DateOfBirth = DateTime.UtcNow,
                Address = Faker.Address.StreetAddress()
            };
        }
        private static OwnerForUpdateRequest OwnerForUpdateObject()
        {
            return new OwnerForUpdateRequest
            {
                Name = Faker.Name.FullName(),
                DateOfBirth = DateTime.UtcNow,
                Address = Faker.Address.StreetName()
            };
        }
        #endregion
    }
}
