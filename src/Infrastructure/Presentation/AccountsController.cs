using Contracts.Accounts;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace Presentation
{
    [ApiController]
    [Route("api/owners/{ownerId:guid}/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public AccountsController(IManagerService managerService) => _managerService = managerService;

        [HttpGet]
        public async Task<IActionResult> GetAccounts(Guid ownerId, CancellationToken cancellationToken)
        {
            var accounts = await _managerService.AccountService.GetAccountsByOwnerAsync(ownerId, cancellationToken);
            return Ok(accounts);
        }

        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetAccountById(Guid ownerId, Guid accountId, CancellationToken cancellationToken)
        {
            var account = await _managerService.AccountService.GetAccountByIdAsync(ownerId, accountId, cancellationToken);
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(Guid ownerId, [FromBody] AccountForCreationRequest accountForCreation, CancellationToken cancellationToken)
        {
            var response = await _managerService.AccountService.CreateAccountAsync(ownerId, accountForCreation, cancellationToken);
            return CreatedAtAction(nameof(GetAccountById), new { ownerId = response.OwnerId, accountId = response.Id }, response);
        }

        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult> DeleteAccount(Guid ownerId, Guid accountId, CancellationToken cancellationToken)
        {
            await _managerService.AccountService.DeleteAccountAsync(ownerId, accountId, cancellationToken);
            return NoContent();
        }
    }
}
