using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Appointments
{
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IMediator _mediator;

        public AppointmentsController(ILogger<AppointmentsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // gets currently logged in patients' appointments;
        [Authorize("read:appointments")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> Get()
        {
            var res = await _mediator.Send(new Get.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }

        [Authorize("read:appointments")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            if (res is null)
                return NotFound();
            return Ok(res);
        }

        [Authorize("write:appointments")]
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Post([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = res.AppointmentId }, res);
        }
    }
}