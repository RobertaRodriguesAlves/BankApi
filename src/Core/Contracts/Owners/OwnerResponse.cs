using Contracts.Accounts;

namespace Contracts.Owners
{
    public class OwnerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public IEnumerable<AccountResponse> Accounts { get; set; }
    }
}
