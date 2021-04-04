using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    /// <summary>
    /// Controller for pain events belonging to a patient.
    /// </summary>
    [ApiController]
    [Route("api/patients")]
    public class PatientsPainEventsController : ControllerBase
    {
        private readonly ILogger<PatientsPainEventsController> _logger;
        private readonly IMediator _mediator;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="mediator">Mediator</param>
        public PatientsPainEventsController(ILogger<PatientsPainEventsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        /// <summary>
        /// Obtains pain events for the currently logged in patient.
        /// </summary>
        /// <returns>Pain events belonging to the currently logged in patient.</returns>
        [Authorize("read:pain")]
        [HttpGet("@me/pain")]
        public async Task<ActionResult<IEnumerable<PainEventDto>>> GetForMe()
        {
            var res = _mediator.Send(new PainEvents.Get.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }
        
        /// <summary>
        /// Obtains pain events by patient ID.
        /// </summary>
        /// <param name="query">Patient ID obtained from the request route.</param>
        /// <returns>Pain events belonging to the patient.</returns>
        [Authorize("read:allpatients")]
        [HttpGet("{patientId}/meals")]
        public async Task<ActionResult<IEnumerable<PainEventDto>>> Get([FromRoute] PainEvents.Get.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
        
        /// <summary>
        /// Obtains a pain event belonging to the currently logged in patient by ID.
        /// </summary>
        /// <param name="query">The pain event ID obtained from the route URL.</param>
        /// <returns>Pain event.</returns>
        [Authorize("read:pain")]
        [HttpGet("@me/pain/{id}")]
        public async Task<ActionResult<PainEventDto>> GetForMeById([FromRoute] PainEvents.GetById.Query query)
        {
            var res = await _mediator.Send(query);

            if (!res.AuthSucceeded) return Forbid();
            if (res.Payload is null)
            {
                return NotFound();
            }

            return Ok(res);
        }

        [Authorize("read:pain")]
        [HttpGet("@me/pain/recent/avgs")]
        public async Task<ActionResult<PainEvents.GetRecentAvgs.Result>> GetRecentAvgsForMe()
        {
            var res = await _mediator.Send(new PainEvents.GetRecentAvgs.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }

        [Authorize("write:pain")]
        [HttpPost("@me/pain")]
        public async Task<ActionResult<PainEventDto>> PostForMe([FromBody] PainEvents.Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetForMeById), new {id = res.PainEventId}, res);
        }
    }
}