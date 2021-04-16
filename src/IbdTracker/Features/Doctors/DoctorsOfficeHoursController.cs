using System.Threading.Tasks;
using IbdTracker.Features.Doctors.OfficeHours;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Doctors
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsOfficeHoursController : ControllerBase
    {
        private readonly ILogger<DoctorsOfficeHoursController> _logger;
        private readonly IMediator _mediator;

        public DoctorsOfficeHoursController(ILogger<DoctorsOfficeHoursController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [Authorize("write:officehours")]
        [HttpPut]
        public async Task<ActionResult> UpdateMyOfficeHours(
            [FromBody] ChangeOfficeHours.HttpRequestBody httpRequestBody) =>
            await _mediator.Send(new ChangeOfficeHours.Command(User.Identity!.Name!, httpRequestBody));

        [Authorize("write:alldoctors")]
        [HttpPut("{doctorId}/officehours")]
        public async Task<ActionResult> UpdateOfficeHours([FromRoute] string doctorId,
            [FromBody] ChangeOfficeHours.HttpRequestBody httpRequestBody) =>
            await _mediator.Send(new ChangeOfficeHours.Command(doctorId, httpRequestBody));
    }
}