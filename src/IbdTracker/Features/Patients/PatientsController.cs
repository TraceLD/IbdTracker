using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly ILogger<PatientsController> _logger;
        private readonly IMediator _mediator;

        public PatientsController(ILogger<PatientsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        // gets patient object corresponding to currently logged in user;
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Patient>> Get()
        {
            var res = await _mediator.Send(new Get.Query {AuthId = User.Identity?.Name});
            if (res is null) return NotFound();
            return Ok(res);
        }
        
        // gets patients assigned to currently logged in doctor;
        [Authorize("read:assignedpatients")]
        [HttpGet("assigned")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetAssigned()
        {
            var res = await _mediator.Send(new GetAssigned.Query{AuthId = User.Identity?.Name});
            return Ok(res);
        }
        
        // gets all patients
        [Authorize("read:allpatients")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
        {
            var res = await _mediator.Send(new GetAll.Query());
            return Ok(res);
        }
    }
}