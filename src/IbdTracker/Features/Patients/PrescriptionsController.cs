using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.Prescriptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    [ApiController]
    [Route("api/patients/prescriptions")]
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
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> Get()
        {
            var res = await _mediator.Send(new Prescriptions.Get.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }

        [Authorize("read:prescriptions")]
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetActive()
        {
            var res = await _mediator.Send(new GetActive.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }
    }
}