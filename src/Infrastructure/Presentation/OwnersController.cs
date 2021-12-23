using Contracts.Owners;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace Presentation
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerrService;

        public OwnersController(IOwnerService ownerService) => _ownerrService = ownerService;

        [HttpGet]
        public async Task<IActionResult> GetOwners(CancellationToken cancellationToken)
        {
            var owners = await _ownerrService.GetOwnersAsync(cancellationToken);
            return Ok(owners);
        }

        [HttpGet("{ownerId:guid}")]
        public async Task<IActionResult> GetOwnerById(Guid ownerId, CancellationToken cancellationToken)
        {
            var owner = await _ownerrService.GetOwnerByIdAsync(ownerId, cancellationToken);
            return Ok(owner);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerForCreationRequest ownerForCreation)
        {
            var owner = await _ownerrService.CreateOwnerAsync(ownerForCreation);
            return CreatedAtAction(nameof(GetOwnerById), new { ownerId = owner.Id }, owner);
        }

        [HttpPut("{ownerId:guid}")]
        public async Task<IActionResult> UpdateOwner(Guid ownerId, [FromBody] OwnerForUpdateRequest ownerForUpdate, CancellationToken cancellationToken)
        {
            await _ownerrService.UpdateOwnerAsync(ownerId, ownerForUpdate, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{ownerId:guid}")]
        public async Task<IActionResult> DeleteOwner(Guid ownerId, CancellationToken cancellationToken)
        {
            await _ownerrService.DeleteOwnerAsync(ownerId, cancellationToken);
            return NoContent();
        }
    }
}
