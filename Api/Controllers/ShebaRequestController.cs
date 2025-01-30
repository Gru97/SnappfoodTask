using Application;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("sheba-request")]
    public class ShebaRequestController : ControllerBase
    {
        private readonly ILogger<ShebaRequestController> _logger;
        private readonly IPayaTransferApplicationService _applicationService;

        public ShebaRequestController(ILogger<ShebaRequestController> logger, IPayaTransferApplicationService applicationService)
        {
            _logger = logger;
            _applicationService = applicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShebaRequestDto shebaRequestDto, CancellationToken token)
        {
            var result =  await _applicationService.CreateShebaRequest(shebaRequestDto, token);  
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);  //This should happen in a general way and based on some configuration, not here
        }
    }
}
