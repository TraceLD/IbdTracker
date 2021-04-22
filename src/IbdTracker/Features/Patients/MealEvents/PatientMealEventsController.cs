using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.MealEvents
{
    [ApiController]
    [Route("api/patients/@me/meals/events")]
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealEventDto>>> GetForMe()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }

        [Authorize("read:meals")]
        [HttpGet("{mealEventId}")]
        public async Task<ActionResult<MealEventDto>> GetForMeById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }
        
        [Authorize("write:meals")]
        [HttpPost]
        public async Task<ActionResult<MealEventDto>> PostForMe([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetForMeById), new {id = res.MealId}, res);
        }
    }
}