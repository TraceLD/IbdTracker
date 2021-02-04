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
        ///
        /// Requires "read:meals" policy.
        /// </summary>
        /// <returns>Meals for the currently logged in patient.</returns>
        [Authorize("read:meals")]
        [HttpGet("@me/meals")]
        public async Task<ActionResult<IEnumerable<MealDto>>> Get()
        {
            var res = await _mediator.Send(new Meals.Get.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }
        
        /// <summary>
        /// Obtains meals by patient ID.
        ///
        /// Requires "read:allpatients" policy.
        /// </summary>
        /// <param name="query">Patient ID obtained from the request route.</param>
        /// <returns>Meals for the given patient.</returns>
        [Authorize("read:allpatients")]
        [HttpGet("{patientId}/meals")]
        public async Task<ActionResult<IEnumerable<MealDto>>> GetRecent([FromRoute] Meals.Get.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
    }
}