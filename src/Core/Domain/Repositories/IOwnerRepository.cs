using Domain.Entities;

namespace Domain.Repositories
{
    public interface IOwnerRepository : IBaseRepository<Owner>
    {
        Task<IEnumerable<Owner>> GetOwnersAsync(CancellationToken cancellationToken = default);
        Task<Owner> GetOwnerByIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);
    }
}
