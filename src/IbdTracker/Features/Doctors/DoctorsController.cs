using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
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

        [Authorize("read:doctor")]
        [HttpGet("@me")]
        public async Task<ActionResult<DoctorDto>> GetMe()
        {
            var res = await _mediator.Send(new GetCurrent.Query());
            return res is null ? Unauthorized() : Ok(res);
        }

        [HttpGet]
        public async Task<ActionResult<IList<DoctorDto>>> Get()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }

        [HttpGet("{doctorId}")]
        public async Task<ActionResult<DoctorDto>> GetById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? Unauthorized() : Ok(res);
        }

        [Authorize("read:assignedpatients")]
        [HttpGet("@me/patients")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetMyPatients()
        {
            var res = await _mediator.Send(new GetAssignedPatients.Query());
            return Ok(res);
        }

        [Authorize]
        [HttpPost("@me/register")]
        public async Task<ActionResult> Register(Register.Command command) =>
            await _mediator.Send(command);
    }
}