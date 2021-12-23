using Contracts.Owners;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;

namespace Service
{
    public sealed class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OwnerService(IOwnerRepository ownerRepository,
                            IUnitOfWork unitOfWork)
        {
            _ownerRepository = ownerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OwnerResponse> CreateOwnerAsync(OwnerForCreationRequest ownerCreationRequest, CancellationToken cancellationToken = default)
        {
            var owner = ownerCreationRequest.Adapt<Owner>();
            _ownerRepository.CreateOwner(owner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return owner.Adapt<OwnerResponse>();
        }

        public async Task DeleteOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var owner = await _ownerRepository.GetOwnerByIdAsync(ownerId);
            if (owner is null)
                throw new OwnerNotFoundException(ownerId);
            _ownerRepository.DeleteOwner(owner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<OwnerResponse> GetOwnerByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var owner = await _ownerRepository.GetOwnerByIdAsync(ownerId);
            if (owner is null)
                throw new OwnerNotFoundException(ownerId);
            return owner.Adapt<OwnerResponse>();
        }

        public async Task<IEnumerable<OwnerResponse>> GetOwnersAsync(CancellationToken cancellationToken = default)
        {
            var owners = await _ownerRepository.GetOwnersAsync(cancellationToken);
            return owners.Adapt<IEnumerable<OwnerResponse>>();
        }

        public async Task UpdateOwnerAsync(Guid ownerId, OwnerForUpdateRequest ownerForUpdateRequest, CancellationToken cancellationToken = default)
        {
            var owner = await _ownerRepository.GetOwnerByIdAsync(ownerId, cancellationToken);
            if (owner is null)
                throw new OwnerNotFoundException(ownerId);
            owner.Name = ownerForUpdateRequest.Name;
            owner.Address = ownerForUpdateRequest.Address;
            owner.DateOfBirth = ownerForUpdateRequest.DateOfBirth;
            _ownerRepository.UpdateOwner(owner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
