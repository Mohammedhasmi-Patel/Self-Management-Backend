using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.DTO.Company;
using SelfManagement.Application.ServiceInterface.Company;
using System.Security.Claims;

namespace SelfManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesService _companiesService;

        public CompaniesController(ICompaniesService companiesService)
        {
            _companiesService = companiesService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCompaniesByUserAsync([FromQuery] GetCompaniesRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized("Invalid or missing User ID in token.");
            }
            var response = await _companiesService.GetAllCompaniesByUserAsync(request, userId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyAsync(CreateCompanyRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!Guid.TryParse(userIdStr,out var userId))
            {
                return Unauthorized("Invalid or missing User ID in token.");
            }
            var response = await _companiesService.CreateCompanyAsync(request, userId);
            return Ok(response);
        }
    }
}
