using System.Threading.Tasks;
using IbdTracker.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Accounts
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IMediator _mediator;

        public AccountsController(ILogger<AccountsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("@me/accountType")]
        public async Task<ActionResult<int>> GetAccountType()
        {
            var res = await _mediator.Send(new GetAccountType.Query(User.Identity?.Name));
            return Ok(res);
        }
    }
}