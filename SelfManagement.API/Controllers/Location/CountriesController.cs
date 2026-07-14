using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.ServiceInterface.Location;

namespace SelfManagement.API.Controllers.Location
{
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesService _countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCompaniesAsync()
        {
            var response = await _countriesService.GetCountryListDropdownsAsync();
            return Ok(response);
        }
    }
}
