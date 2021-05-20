using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.InformationRequests
{
    [ApiController]
    [Route("api/patients/@me/informationRequests")]
    public class PatientInformationRequestsController : ControllerBase
    {
        private readonly ILogger<PatientInformationRequestsController> _logger;
        private readonly IMediator _mediator;

        public PatientInformationRequestsController(ILogger<PatientInformationRequestsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{informationRequestId}")]
        public async Task<ActionResult<InformationRequestDto>> GetOneForMeById([FromRoute] GetOne.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<InformationRequestDto>>> GetActiveForMe()
        {
            var res = await _mediator.Send(new GetActive.Query());
            return Ok(res);
        }

        [HttpPut("{informationRequestId}")]
        public async Task<ActionResult> Put([FromRoute] Guid informationRequestId, [FromBody] Put.Command command)
        {
            if (informationRequestId != command.InformationRequestId)
            {
                return BadRequest();
            }

            return await _mediator.Send(command);
        }
    }
}