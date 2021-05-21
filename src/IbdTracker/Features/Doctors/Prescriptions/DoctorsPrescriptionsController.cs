using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Doctors.Prescriptions
{
    [ApiController]
    [Route("api/doctors/@me/prescriptions")]
    public class DoctorsPrescriptionsController : ControllerBase
    {
        private readonly ILogger<DoctorsPrescriptionsController> _logger;
        private readonly IMediator _mediator;

        public DoctorsPrescriptionsController(ILogger<DoctorsPrescriptionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("write:prescriptions")]
        [HttpPost]
        public async Task<ActionResult> Prescribe([FromBody] Prescribe.Command command) =>
            await _mediator.Send(command);

        [Authorize("read:prescriptions")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptions([FromQuery] Get.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [Authorize("read:prescriptions")]
        [HttpGet("{prescriptionId}")]
        public async Task<ActionResult<PrescriptionDto>> GetPrescriptionById(
            [FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }
    }
}