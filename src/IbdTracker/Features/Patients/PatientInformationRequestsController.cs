using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.InformationRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    [ApiController]
    [Route("api/patients")]
    public class PatientInformationRequestsController : ControllerBase
    {
        private readonly ILogger<PatientInformationRequestsController> _logger;
        private readonly IMediator _mediator;

        public PatientInformationRequestsController(ILogger<PatientInformationRequestsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("@me/informationRequests/{id}")]
        public async Task<ActionResult<InformationRequestDto>> GetOneForMeById(Guid id)
        {
            var res = await _mediator.Send(new GetOne.Query(User.Identity!.Name!, id));
            return res is null ? NotFound() : Ok(res);
        }

        [HttpGet("@me/informationRequests/active")]
        public async Task<ActionResult<IEnumerable<InformationRequestDto>>> GetActiveForMe()
        {
            var res = await _mediator.Send(new GetActive.Query(User.Identity!.Name!));
            return Ok(res);
        }

        [HttpPut("@me/informationRequests/{id}")]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] Put.Command command)
        {
            if (id != command.InformationRequestId)
            {
                return BadRequest();
            }

            if (User.Identity?.Name is null || User.Identity.Name != command.PatientId)
            {
                return Unauthorized();
            }

            return await _mediator.Send(command);
        }
    }
}