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
        [HttpGet("isregistered")]
        public async Task<ActionResult<IsRegistered.Result>> IsRegistered()
        {
            var res = await _mediator.Send(new IsRegistered.Query {AuthId = User.Identity?.Name});
            return Ok(res);
        }
    }
}