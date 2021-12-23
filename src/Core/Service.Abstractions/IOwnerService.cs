using Contracts.Owners;

namespace Service.Abstractions
{
    public interface IOwnerService
    {
        Task<IEnumerable<OwnerResponse>> GetOwnersAsync(CancellationToken cancellationToken = default);
        Task<OwnerResponse> GetOwnerByIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<OwnerResponse> CreateOwnerAsync(OwnerForCreationRequest ownerCreationRequest, CancellationToken cancellationToken = default);
        Task UpdateOwnerAsync(Guid ownerId, OwnerForUpdateRequest ownerForUpdateRequest, CancellationToken cancellationToken = default);
        Task DeleteOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
    }
}
