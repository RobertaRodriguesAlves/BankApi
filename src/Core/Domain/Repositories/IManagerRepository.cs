namespace Domain.Repositories
{
    public interface IManagerRepository
    {
        IOwnerRepository OwnerRepository { get; }
        IAccountRepository AccountRepository { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}
