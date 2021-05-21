using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    [ApiController]
    [Route("api/patients/research")]
    public class PatientsResearchController : ControllerBase
    {
        private readonly ILogger<PatientsResearchController> _logger;
        private readonly IMediator _mediator;

        public PatientsResearchController(ILogger<PatientsResearchController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [Authorize("read:researchdata")]
        [HttpGet]
        public async Task<ActionResult<IList<GetAnonymisedDataForResearch.Result>>> GetResearchData()
        {
            var res = await _mediator.Send(new GetAnonymisedDataForResearch.Query());
            return Ok(res);
        }
    }
}