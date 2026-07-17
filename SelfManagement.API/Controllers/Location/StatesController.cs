using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.ServiceInterface.Location;

namespace SelfManagement.API.Controllers.Location
{
    [Route("api/states")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StatesController(IStateService stateService)
        {
            _stateService = stateService;
        }
        [HttpGet]
        public async Task<IActionResult> GetStatesByCountryIdAsync(Guid countryId)
        {
            var response = await _stateService.GetStatesByCountryIdAsync(countryId);
            return Ok(response);
        }
    }
}
