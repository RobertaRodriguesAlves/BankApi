namespace Contracts.Accounts
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime DateCreated { get; set; }
        public string AccountType { get; set; }
    }
}
