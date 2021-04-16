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
    public class DoctorsInformationRequestsController : ControllerBase
    {
        private readonly ILogger<DoctorsInformationRequestsController> _logger;
        private readonly IMediator _mediator;

        public DoctorsInformationRequestsController(ILogger<DoctorsInformationRequestsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("write:informationrequests")]
        [HttpPost("@me/informationRequests")]
        public async Task<ActionResult<InformationRequestDto>> PostInformationRequest(
            [FromBody] RequestData.Command command)
        {
            var res = await _mediator.Send(command);
            return Ok(res);
        }
    }
}