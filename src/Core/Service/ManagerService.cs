using Domain.Repositories;
using Service.Abstractions;

namespace Service
{
    public sealed class ManagerService : IManagerService
    {
        private readonly Lazy<IAccountService> _lazyAccountService;
        public ManagerService(IManagerRepository managerRepository)
        {
            _lazyAccountService = new Lazy<IAccountService>(() => new AccountService(managerRepository));
        }
        public IAccountService AccountService => _lazyAccountService.Value;
    }
}
