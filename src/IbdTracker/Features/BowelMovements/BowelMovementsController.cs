using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.BowelMovements
{
    [Route("api/bms")]
    public class BowelMovementsController : ControllerBase
    {
        private readonly ILogger<BowelMovementsController> _logger;
        private readonly IMediator _mediator;

        public BowelMovementsController(ILogger<BowelMovementsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:bms")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BowelMovementEventDto>> GetById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            if (res is null)
                return NotFound();
            return Ok(res);
        }
        
        [Authorize("write:bms")]
        [HttpPost]
        public async Task<ActionResult<BowelMovementEventDto>> Post([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = res.BowelMovementEventId }, res);
        }
    }
}