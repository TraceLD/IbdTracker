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
            var res = await _mediator.Send(User.Identity!.Name!);
            return res is null ? Unauthorized() : Ok(res);
        }
        
        [Authorize("read:alldoctors")]
        [HttpGet("{doctorId}")]
        public async Task<ActionResult<DoctorDto>> GetById(GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }

        [Authorize("read:assignedpatients")]
        [HttpGet("@me/patients")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetMyPatients()
        {
            var res = await _mediator.Send(new GetAssignedPatients.Query(User.Identity!.Name!));
            return Ok(res);
        }
    }
}