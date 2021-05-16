using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Doctors.Prescriptions
{
    [ApiController]
    [Route("doctors/@me/prescriptions")]
    public class DoctorsPrescriptionsController : ControllerBase
    {
        private readonly ILogger<DoctorsPrescriptionsController> _logger;
        private readonly IMediator _mediator;

        public DoctorsPrescriptionsController(ILogger<DoctorsPrescriptionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Prescribe([FromBody] Prescribe.Command command) =>
            await _mediator.Send(command);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptions([FromQuery] Get.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpGet("{prescriptionId}")]
        public async Task<ActionResult<IEnumerable<GetById.Result>>> GetPrescriptionById(
            [FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }
    }
}