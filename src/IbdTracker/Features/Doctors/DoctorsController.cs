using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Doctors.OfficeHours;
using IbdTracker.Features.Doctors.Patients;
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

        [Authorize]
        [HttpGet("@me")]
        public async Task<ActionResult<DoctorDto>> GetMe()
        {
            var res = await _mediator.Send(new GetById.Query {DoctorId = User.Identity!.Name!});
            return res is null ? Unauthorized() : Ok(res);
        }

        [Authorize("read:assignedpatients")]
        [HttpGet("@me/patients")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetMyPatients()
        {
            var res = await _mediator.Send(new GetAssignedPatients.Query(User.Identity!.Name!));
            return Ok(res);
        }

        [Authorize]
        [HttpPost("@me/informationRequests")]
        public async Task<ActionResult<InformationRequestDto>> PostInformationRequest(
            [FromBody] RequestData.Command command)
        {
            var res = await _mediator.Send(command);
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