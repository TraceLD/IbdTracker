using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Features.Doctors.Appointments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Doctors
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
            [FromRoute] string doctorId, [FromQuery] DateTime day) =>
            Ok(await _mediator.Send(new GetAvailableAppointmentsOnDay.Query(doctorId, day)));

        [HttpGet("{doctorId}/appointments/isAvailable")]
        public async Task<ActionResult> IsAvailable([FromRoute] string doctorId, [FromQuery] DateTime dateTime) =>
            Ok(await _mediator.Send(new IsAppointmentAvailable.Query(doctorId, dateTime)));
    }
}