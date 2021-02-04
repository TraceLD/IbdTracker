using System.Collections;
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
    /// <summary>
    /// Controller for obtaining BM's belonging to a patient.
    /// </summary>
    [Route("/api/patients")]
    public class PatientsBowelMovementsController : ControllerBase
    {
        private readonly ILogger<PatientsBowelMovementsController> _logger;
        private readonly IMediator _mediator;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="mediator">Mediator</param>
        public PatientsBowelMovementsController(ILogger<PatientsBowelMovementsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        /// <summary>
        /// Obtains bowel movement events for the currently logged in patient.
        ///
        /// Requires "read:bms" policy.
        /// </summary>
        /// <returns>Bowel movement events for the currently logged in patient.</returns>
        [Authorize("read:bms")]
        [HttpGet("@me/bms/recent")]
        public async Task<ActionResult<IEnumerable<BowelMovementEventDto>>> GetRecentForMe()
        {
            var res = await _mediator.Send(new GetRecent.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }
        
        /// <summary>
        /// Obtains bowel movement events by patient ID.
        ///
        /// Requires "read:allpatients" policy.
        /// </summary>
        /// <param name="query">Patient ID obtained from the request route.</param>
        /// <returns>Bowel movement events for the given patient.</returns>
        [Authorize("read:allpatients")]
        [HttpGet("{patientId}/recent")]
        public async Task<ActionResult<IEnumerable<BowelMovementEventDto>>> GetRecent([FromRoute] GetRecent.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
    }
}