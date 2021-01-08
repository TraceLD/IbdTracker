using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Prescriptions
{
    [Route("api/prescriptions")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly ILogger<PrescriptionsController> _logger;
        private readonly IMediator _mediator;

        public PrescriptionsController(ILogger<PrescriptionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:prescriptions")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Get.Result>>> Get()
        {
            var res = await _mediator.Send(new Get.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }
    }
}