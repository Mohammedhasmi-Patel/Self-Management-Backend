using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.DTO.Project;
using SelfManagement.Application.ServiceInterface.Project;
using System.Security.Claims;

namespace SelfManagement.API.Controllers
{
    [Authorize]
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectsByUserAsync([FromQuery] GetProjectsRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized("Invalid or missing User ID in token.");
            }

            var response = await _projectsService.GetAllProjectsByUserAsync(request, userId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync(CreateProjectRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized("Invalid or missing User ID in token.");
            }

            var response = await _projectsService.CreateProjectAsync(request, userId);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProjectAsync(UpdateProjectRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized("Invalid or missing User ID in token.");
            }

            var response = await _projectsService.UpdateProjectAsync(request, userId);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProjectAsync(Guid id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized("Invalid or missing User ID in token.");
            }

            var response = await _projectsService.DeleteProjectByIdAsync(id, userId);
            return StatusCode(201, response);
        }
    }
}
