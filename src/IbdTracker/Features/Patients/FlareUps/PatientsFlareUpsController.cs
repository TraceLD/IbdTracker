using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.FlareUps
{
    [ApiController]
    [Route("api/patients/@me/flareups")]
    public class PatientsFlareUpsController : ControllerBase
    {
        private readonly ILogger<PatientsFlareUpsController> _logger;
        private readonly IMediator _mediator;

        public PatientsFlareUpsController(ILogger<PatientsFlareUpsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("analyse")]
        public async Task<ActionResult<FlareUpDetectionResult>> Analyse()
        {
            var res = await _mediator.Send(new Analyse.Query());
            return Ok(res);
        }
    }
}