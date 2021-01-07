using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IEnumerable<Get.Result>>> GetAppointments()
        {
            var res = await _mediator.Send(new Get.Query {PatientId = User.Identity?.Name});
            return Ok(res);
        }
    }
}