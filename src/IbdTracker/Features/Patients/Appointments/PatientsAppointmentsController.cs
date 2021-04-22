using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.Appointments
{
    [ApiController]
    [Route("api/patients/@me/appointments")]
    public class PatientsAppointmentsController : ControllerBase
    {
        private readonly ILogger<PatientsAppointmentsController> _logger;
        private readonly IMediator _mediator;

        public PatientsAppointmentsController(ILogger<PatientsAppointmentsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:appointments")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetForMe()
        {
            var res = await _mediator.Send(new GetAll.Query());
            return Ok(res);
        }

        [Authorize("read:appointments")]
        [HttpGet("{appointmentId}")]
        public async Task<ActionResult<AppointmentDto>> GetOneForMeById([FromRoute] GetOne.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }

        [Authorize("write:appointments")]
        [HttpPut("{appointmentId}")]
        public async Task<ActionResult> PutForMe([FromRoute] Guid appointmentId, [FromBody] Put.Command command)
        {
            if (appointmentId != command.AppointmentId)
            {
                return BadRequest();
            }

            return await _mediator.Send(command);
        }

        [Authorize("write:appointments")]
        [HttpDelete("{appointmentId}")]
        public async Task<ActionResult> DeleteAppointmentForMe(Cancel.Command command) =>
            await _mediator.Send(command);

        [Authorize("write:appointments")]
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> PostAppointmentForMe([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOneForMeById), new {appointmentId = res.AppointmentId}, res);
        }
    }
}