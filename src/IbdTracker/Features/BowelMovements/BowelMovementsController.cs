using System;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.BowelMovements
{
    [ApiController]
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

        [Authorize("read:allbms")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BowelMovementEventDto>> GetById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }

        [Authorize("write:allbms")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id = res.BowelMovementEventId}, res);
        }

        [Authorize("write:allbms")]
        [HttpPut("{id}")]
        public async Task<ActionResult<BowelMovementEventDto>> Put([FromRoute] Guid id, [FromBody] Put.Command command)
        {
            if (id != command.BowelMovementEventId)
            {
                return BadRequest();
            }

            return await _mediator.Send(command);
        }

        [Authorize("write:allbms")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Delete.Command command) =>
            await _mediator.Send(command);
    }
}