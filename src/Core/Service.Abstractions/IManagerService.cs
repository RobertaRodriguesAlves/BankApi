namespace Service.Abstractions
{
    public interface IManagerService
    {
        //IOwnerService OwnerService { get; }
        IAccountService AccountService { get; }
    }
}
