using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.Appointments;
using IbdTracker.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsAppointmentsController : ControllerBase
    {
        private readonly ILogger<PatientsAppointmentsController> _logger;
        private readonly IMediator _mediator;

        public PatientsAppointmentsController(ILogger<PatientsAppointmentsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [Authorize("read:allpatients")]
        [HttpGet("{patientId}/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> Get(string patientId)
        {
            throw new NotImplementedException();
        }
        
        [Authorize("read:appointments")]
        [HttpGet("@me/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetForMe()
        {
            throw new NotImplementedException();
        }

        [Authorize("read:appointments")]
        [HttpGet("@me/appointments/{id}")]
        public async Task<ActionResult<AppointmentDto>> GetOneForMeById(Guid id)
        {
            throw new NotImplementedException();
        }

        [Authorize("write:appointments")]
        [HttpDelete("@me/appointments/{id}")]
        public async Task<ActionResult> DeleteAppointmentForMe(Guid id)
        {
            var patientEmail = User.FindFirst(AppJwtClaims.EmailClaim)?.Value;
            return await _mediator.Send(new Cancel.Command(User.Identity!.Name!, patientEmail, id));
        }

        [Authorize("write:appointments")]
        [HttpPost("@me/appointments")]
        public async Task<ActionResult<AppointmentDto>> PostAppointmentForMe([FromBody] Post.HttpRequestBody requestBody)
        {
            var patientEmail = User.FindFirst(AppJwtClaims.EmailClaim)?.Value;
            var res = await _mediator.Send(new Post.Command(User.Identity!.Name!, patientEmail, requestBody));
            return CreatedAtAction(nameof(GetOneForMeById), new {id = res.AppointmentId}, res);
        }
    }
}