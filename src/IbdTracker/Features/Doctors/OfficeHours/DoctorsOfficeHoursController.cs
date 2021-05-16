using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Features.Doctors.OfficeHours;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Doctors.OfficeHours
{
    [ApiController]
    [Route("api/doctors/@me/officehours")]
    public class DoctorsOfficeHoursController : ControllerBase
    {
        private readonly ILogger<DoctorsOfficeHoursController> _logger;
        private readonly IMediator _mediator;

        public DoctorsOfficeHoursController(ILogger<DoctorsOfficeHoursController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:doctor")]
        public async Task<ActionResult<IList<Core.OfficeHours>>> GetMyOfficeHours()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }

        [Authorize("write:doctor")]
        [HttpPut]
        public async Task<ActionResult> UpdateMyOfficeHours([FromBody] ChangeOfficeHours.Command command) =>
            await _mediator.Send(command);
    }
}