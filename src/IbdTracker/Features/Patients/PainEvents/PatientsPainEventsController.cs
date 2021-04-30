using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.PainEvents
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsPainEventsController : ControllerBase
    {
        private readonly ILogger<PatientsPainEventsController> _logger;
        private readonly IMediator _mediator;
        
        public PatientsPainEventsController(ILogger<PatientsPainEventsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [Authorize("read:pain")]
        [HttpGet("@me/pain")]
        public async Task<ActionResult<IEnumerable<PainEventDto>>> GetForMe()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }

        /// <summary>
        /// Obtains a pain event belonging to the currently logged in patient by ID.
        /// </summary>
        /// <param name="query">The pain event ID obtained from the route URL.</param>
        /// <returns>Pain event.</returns>
        [Authorize("read:pain")]
        [HttpGet("@me/pain/{painEventId}")]
        public async Task<ActionResult<PainEventDto>> GetForMeById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok();
        }

        [Authorize("read:pain")]
        [HttpGet("@me/pain/recent/avgs")]
        public async Task<ActionResult<GetRecentAvgs.Result>> GetRecentAvgsForMe([FromQuery] GetRecentAvgs.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [Authorize("write:pain")]
        [HttpPost("@me/pain")]
        public async Task<ActionResult<PainEventDto>> PostForMe([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetForMeById), new {painEventId = res.PainEventId}, res);
        }
    }
}