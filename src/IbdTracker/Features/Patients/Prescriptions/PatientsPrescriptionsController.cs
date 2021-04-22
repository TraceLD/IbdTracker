using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.Prescriptions
{
    [ApiController]
    [Route("api/patients/prescriptions")]
    public class PatientsPrescriptionsController : ControllerBase
    {
        private readonly ILogger<PatientsPrescriptionsController> _logger;
        private readonly IMediator _mediator;

        public PatientsPrescriptionsController(ILogger<PatientsPrescriptionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:prescriptions")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> Get()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }

        [Authorize("read:prescriptions")]
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetActive()
        {
            var res = await _mediator.Send(new GetActive.Query());
            return Ok(res);
        }
    }
}