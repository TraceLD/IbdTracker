using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Doctors.OfficeHours;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Doctors
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMediator _mediator;

        public DoctorsController(ILogger<DoctorsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetById(string id)
        {
            var res = await _mediator.Send(new GetById.Query{DoctorId = id});
            
            if (res is null)
            {
                return NotFound();
            }

            return Ok(res);
        }

        [Authorize("write:officehours")]
        [HttpPut]
        public async Task<ActionResult> UpdateMyOfficeHours(
            [FromBody] ChangeOfficeHours.HttpRequestBody httpRequestBody) =>
            await _mediator.Send(new ChangeOfficeHours.Command(User.Identity!.Name!, httpRequestBody));

        //[Authorize("write:alldoctors")]
        [HttpPut("{doctorId}/officehours")]
        public async Task<ActionResult> UpdateOfficeHours([FromRoute] string doctorId,
            [FromBody] ChangeOfficeHours.HttpRequestBody httpRequestBody) =>
            await _mediator.Send(new ChangeOfficeHours.Command(doctorId, httpRequestBody));
    }
}