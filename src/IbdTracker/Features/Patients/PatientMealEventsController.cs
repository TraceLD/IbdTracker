using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    [Route("api/patients")]
    public class PatientMealEventsController : ControllerBase
    {
        private readonly ILogger<PatientMealEventsController> _logger;
        private readonly IMediator _mediator;

        public PatientMealEventsController(ILogger<PatientMealEventsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:meals")]
        [HttpGet("@me/meals/events")]
        public async Task<ActionResult<IEnumerable<MealEventDto>>> GetForMe()
        {
            var res = await _mediator.Send(new MealEvents.Get.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }
        
        [Authorize("read:allpatients")]
        [HttpGet("{patientId}/meals/events")]
        public async Task<ActionResult<IEnumerable<MealEventDto>>> Get([FromRoute] MealEvents.Get.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
        
        [Authorize("read:meals")]
        [HttpGet("@me/meals/events/{id}")]
        public async Task<ActionResult<MealEventDto>> GetForMeById([FromRoute] MealEvents.GetById.Query query)
        {
            var res = await _mediator.Send(query);

            if (!res.AuthSucceeded) return Forbid();
            if (res.Payload is null)
            {
                return NotFound();
            }
                
            return Ok(res.Payload);
        }
        
        [Authorize("write:meals")]
        [HttpPost("@me/meals/events")]
        public async Task<ActionResult<MealEventDto>> PostForMe([FromBody] MealEvents.Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetForMeById), new {id = res.MealId}, res);
        }
    }
}