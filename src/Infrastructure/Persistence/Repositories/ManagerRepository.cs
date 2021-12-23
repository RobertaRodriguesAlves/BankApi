using Domain.Repositories;

namespace Persistence.Repositories
{
    public sealed class ManagerRepository : IManagerRepository
    {
        private readonly Lazy<IOwnerRepository> _lazyOwnerRepository;
        private readonly Lazy<IAccountRepository> _lazyAccountRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;
        public ManagerRepository(BankDbContext context)
        {
            _lazyOwnerRepository = new Lazy<IOwnerRepository>(() => new OwnerRepository(context));
            _lazyAccountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
        }
        public IOwnerRepository OwnerRepository => _lazyOwnerRepository.Value;
        public IAccountRepository AccountRepository => _lazyAccountRepository.Value;
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
