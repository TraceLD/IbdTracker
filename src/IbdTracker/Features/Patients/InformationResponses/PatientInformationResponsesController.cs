using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.InformationResponses
{
    [ApiController]
    [Route("api/patients/@me")]
    public class PatientInformationResponsesController : ControllerBase
    {
        private readonly ILogger<PatientInformationResponsesController> _logger;
        private readonly IMediator _mediator;

        public PatientInformationResponsesController(IMediator mediator, ILogger<PatientInformationResponsesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("informationResponses")]
        public async Task<ActionResult> Post([FromBody] Post.Command command) =>
            await _mediator.Send(command);
    }
}