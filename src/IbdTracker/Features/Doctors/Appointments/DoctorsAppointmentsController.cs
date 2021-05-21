using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Doctors.Appointments
{
    [ApiController]
    [Route("/api/doctors")]
    public class DoctorsAppointmentsController : ControllerBase
    {
        private readonly ILogger<DoctorsAppointmentsController> _logger;
        private readonly IMediator _mediator;

        public DoctorsAppointmentsController(ILogger<DoctorsAppointmentsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{doctorId}/appointments/available")]
        public async Task<ActionResult<GetAvailableAppointmentsOnDay.Result>> GetAvailableAppointmentsOnDay(
            [FromRoute] string doctorId, [FromQuery] DateTime day)
        {
            var res = await _mediator.Send(new GetAvailableAppointmentsOnDay.Query(doctorId, day));
            return Ok(res);
        }

        [HttpGet("{doctorId}/appointments/isAvailable")]
        public async Task<ActionResult<IsAppointmentAvailable.Result>> IsAvailable([FromRoute] string doctorId,
            [FromQuery] DateTime dateTime)
        {
            var res = await _mediator.Send(new IsAppointmentAvailable.Query(doctorId, dateTime));
            return Ok(res);
        }

        [Authorize("read:appointments")]
        [HttpGet("@me/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetMyAppointments([FromQuery] Get.Query query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
        
        [Authorize("write:appointments")]
        [HttpDelete("@me/appointments/{appointmentId}")]
        public async Task<ActionResult> CancelAppointmentForMe([FromRoute] Cancel.Command command) =>
            await _mediator.Send(command);

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
        
        [Authorize("read:appointments")]
        [HttpGet("@me/appointments/{appointmentId}")]
        public async Task<ActionResult<AppointmentDto>> GetOneForMeById([FromRoute] GetOne.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }
    }
}