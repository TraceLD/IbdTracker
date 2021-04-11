using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IbdTracker.Features.Patients.BowelMovements;
using Microsoft.AspNetCore.Authorization;

namespace IbdTracker.Features.Patients
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsBowelMovementsController : ControllerBase
    {
        private readonly ILogger<PatientsBowelMovementsController> _logger;
        private readonly IMediator _mediator;
        
        public PatientsBowelMovementsController(ILogger<PatientsBowelMovementsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [Authorize("read:allpatients")]
        [HttpGet("{patientId}/bms/recent")]
        public async Task<ActionResult<IEnumerable<BowelMovementEventDto>>> GetRecent([FromRoute] GetRecent.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
        
        [Authorize("read:bms")]
        [HttpGet("@me/bms/recent")]
        public async Task<ActionResult<IEnumerable<BowelMovementEventDto>>> GetRecentForMe()
        {
            var res = await _mediator.Send(new GetRecent.Query(User.Identity!.Name!));
            return Ok(res);
        }

        [Authorize("read:allpatients")]
        [HttpGet("{patientId}/bms/recent/grouped")]
        public async Task<ActionResult<IEnumerable<GetRecentGrouped.Result>>> GetRecentGrouped(GetRecentGrouped.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
        
        [Authorize("read:bms")]
        [HttpGet("@me/bms/recent/grouped")]
        public async Task<ActionResult<IEnumerable<GetRecentGrouped.Result>>> GetRecentGroupedForMe()
        {
            var res = await _mediator.Send(new GetRecentGrouped.Query(User.Identity!.Name!));
            return Ok(res);
        }

        [Authorize("read:bms")]
        [HttpGet("@me/bms/{id}")]
        public async Task<ActionResult<BowelMovementEventDto>> GetOneForMeById(Guid id)
        {
            var res = await _mediator.Send(new GetOne.Query(User.Identity!.Name!, id));
            return res is null ? NotFound() : Ok(res);
        }

        [Authorize("write:bms")]
        [HttpPost("@me/bms")]
        public async Task<ActionResult<BowelMovementEventDto>> PostBmForMe(
            [FromBody] Post.HttpRequestBody httpRequestBody)
        {
            var res = await _mediator.Send(new Post.Command(User.Identity!.Name!, httpRequestBody));
            return CreatedAtAction(nameof(GetOneForMeById), new {id = res.BowelMovementEventId}, res);
        }

        [Authorize("write:bms")]
        [HttpPut("@me/bms/{id}")]
        public async Task<ActionResult> PutForMe([FromRoute] Guid id, [FromBody] Put.Command command)
        {
            if (id != command.BowelMovementEventId)
            {
                return BadRequest();
            }

            if (User.Identity?.Name is null || !User.Identity.Name.Equals(command.PatientId))
            {
                return Unauthorized();
            }

            return await _mediator.Send(command);
        }

        [Authorize("write:bms")]
        [HttpPut("@me/bms/{id}")]
        public async Task<ActionResult> DeleteForMe([FromRoute] Guid id) =>
            await _mediator.Send(new Delete.Command(User.Identity!.Name!, id));
    }
}