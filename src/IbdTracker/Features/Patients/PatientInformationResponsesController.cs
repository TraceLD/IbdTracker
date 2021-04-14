using System.Threading.Tasks;
using IbdTracker.Features.Patients.InformationResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    [ApiController]
    [Route("api/patients")]
    public class PatientInformationResponsesController : ControllerBase
    {
        private readonly ILogger<PatientInformationResponsesController> _logger;
        private readonly IMediator _mediator;

        public PatientInformationResponsesController(IMediator mediator, ILogger<PatientInformationResponsesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("@me/informationResponses")]
        public async Task<ActionResult> Post([FromBody] Post.HttpRequestBody httpRequestBody) =>
            await _mediator.Send(new Post.Command(User.Identity!.Name!, httpRequestBody));
    }
}