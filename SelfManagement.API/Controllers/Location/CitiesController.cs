using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.ServiceInterface.Location;

namespace SelfManagement.API.Controllers.Location
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesService _citiesService;

        public CitiesController(ICitiesService citiesService)
        {
            _citiesService = citiesService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCitiesByStateIdAsync(Guid stateId)
        {
            var response = await _citiesService.GetCitiesByStateIdAsync(stateId);
            return Ok(response);
        }
    }
}
