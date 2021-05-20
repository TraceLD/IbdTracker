using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.BowelMovements
{
    [ApiController]
    [Route("api/patients/@me/bms")]
    public class PatientsBowelMovementsController : ControllerBase
    {
        private readonly ILogger<PatientsBowelMovementsController> _logger;
        private readonly IMediator _mediator;
        
        public PatientsBowelMovementsController(ILogger<PatientsBowelMovementsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:bms")]
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<BowelMovementEventDto>>> GetRecentForMe()
        {
            var res = await _mediator.Send(new GetRecent.Query());
            return Ok(res);
        }

        [Authorize("read:bms")]
        [HttpGet("recent/grouped")]
        public async Task<ActionResult<IEnumerable<GetRecentGrouped.Result>>> GetRecentGroupedForMe([FromQuery] GetRecentGrouped.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [Authorize("read:bms")]
        [HttpGet("{bowelMovementEventId}")]
        public async Task<ActionResult<BowelMovementEventDto>> GetOneForMeById(GetOne.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }

        [Authorize("write:bms")]
        [HttpPost]
        public async Task<ActionResult<BowelMovementEventDto>> PostBmForMe(
            [FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOneForMeById), new {bowelMovementEventId = res.BowelMovementEventId}, res);
        }

        [Authorize("write:bms")]
        [HttpPut("{bowelMovementEventId}")]
        public async Task<ActionResult> PutForMe([FromRoute] Guid bowelMovementEventId, [FromBody] Put.Command command)
        {
            if (bowelMovementEventId != command.BowelMovementEventId)
            {
                return BadRequest();
            }

            return await _mediator.Send(command);
        }

        [Authorize("write:bms")]
        [HttpDelete("{bowelMovementEventId}")]
        public async Task<ActionResult> DeleteForMe([FromRoute] Delete.Command command) =>
            await _mediator.Send(command);
    }
}