using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public sealed class OwnerRepository : BaseRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(BankDbContext context) : base(context) { }

        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }

        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }

        public async Task<Owner> GetOwnerByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            return await FindByCondition(owner => owner.Id.Equals(ownerId)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Owner>> GetOwnersAsync(CancellationToken cancellationToken = default)
        {
            return await FindAll().OrderBy(owner => owner.Name).ToListAsync();
        }

        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }
    }
}
