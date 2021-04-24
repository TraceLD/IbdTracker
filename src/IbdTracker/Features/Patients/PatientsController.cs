using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    [ApiController]
    [Route("api/patients/@me")]
    public class PatientsController : ControllerBase
    {
        private readonly ILogger<PatientsController> _logger;
        private readonly IMediator _mediator;

        public PatientsController(ILogger<PatientsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:patient")]
        [HttpGet]
        public async Task<ActionResult<PatientDto>> Get()
        {
            var res = await _mediator.Send(new GetCurrent.Query());
            return res is null ? NotFound() : Ok(res);
        }
    }
}