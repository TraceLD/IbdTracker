using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.Meals
{
    /// <summary>
    /// Controller for meals belonging to to a patient.
    /// </summary>
    [ApiController]
    [Route("api/patients/@me/meals")]
    public class PatientsMealsController : ControllerBase
    {
        private readonly ILogger<PatientsMealsController> _logger;
        private readonly IMediator _mediator;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="mediator">Mediator</param>
        public PatientsMealsController(ILogger<PatientsMealsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [Authorize("read:meals")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealDto>>> GetForMe()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }
        
        [Authorize("read:meals")]
        [HttpGet("{mealId}")]
        public async Task<ActionResult<MealDto>> GetForMeById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }
        
        [Authorize("write:meals")]
        [HttpPost]
        public async Task<ActionResult<MealDto>> PostForMe([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetForMeById), new {mealId = res.MealId}, res);
        }
    }
}