using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.Meals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    /// <summary>
    /// Controller for meals belonging to to a patient.
    /// </summary>
    [Route("api/patients")]
    public class PatientMealsController : ControllerBase
    {
        private readonly ILogger<PatientMealsController> _logger;
        private readonly IMediator _mediator;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="mediator">Mediator</param>
        public PatientMealsController(ILogger<PatientMealsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        /// <summary>
        /// Obtains meals for the currently logged in patient.
        /// </summary>
        /// <returns>Meals for the currently logged in patient.</returns>
        [Authorize("read:meals")]
        [HttpGet("@me/meals")]
        public async Task<ActionResult<IEnumerable<MealDto>>> GetForMe()
        {
            var res = await _mediator.Send(new Meals.Get.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }
        
        /// <summary>
        /// Obtains meals by patient ID.
        /// </summary>
        /// <param name="query">Patient ID obtained from the request route.</param>
        /// <returns>Meals for the given patient.</returns>
        [Authorize("read:allpatients")]
        [HttpGet("{patientId}/meals")]
        public async Task<ActionResult<IEnumerable<MealDto>>> Get([FromRoute] Meals.Get.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
        
        /// <summary>
        /// Obtains a meal belonging to the currently logged in patient by ID.
        /// </summary>
        /// <param name="id">The meal ID obtained from the route URL.</param>
        /// <returns></returns>
        [Authorize("read:meals")]
        [HttpGet("@me/meals/{id}")]
        public async Task<ActionResult<MealDto>> GetForMeById([FromRoute] Guid id)
        {
            var res = await _mediator.Send(new GetById.Query{MealId = id, PatientId = User.Identity?.Name});

            if (!res.AuthSucceeded) return Forbid();
            if (res.Payload is null)
            {
                return NotFound();
            }
                
            return Ok(res.Payload);
        }
        
        /// <summary>
        /// Creates a new meal for the currently logged in patient.
        /// </summary>
        /// <param name="command">Meal to be created.</param>
        /// <returns>CreatedAtAction representing creation of the meal.</returns>
        [Authorize("write:meals")]
        [HttpPost("@me/meals")]
        public async Task<ActionResult<MealDto>> PostForMe([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetForMeById), new {id = res.MealId}, res);
        }
    }
}