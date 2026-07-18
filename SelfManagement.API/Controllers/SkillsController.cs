using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.DTO.Skills;
using SelfManagement.Application.ServiceInterface.Skill;

namespace SelfManagement.API.Controllers
{
    [Authorize]
    [Route("api/skills")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillsService _skillsService;

        public SkillsController(ISkillsService skillsService)
        {
            _skillsService = skillsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSkillsAsync([FromQuery] GetSkillsRequest request)
        {
            var response = await _skillsService.GetAllSkillsAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkillAsync(CreateSkillRequest request)
        {
            var response = await _skillsService.CreateSkillAsync(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSkillAsync(UpdateSkillRequest request)
        {
            var response = await _skillsService.UpdateSkillAsync(request);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSkillAsync(Guid id)
        {
            var response = await _skillsService.DeleteSkillByIdAsync(id);
            return StatusCode(201, response);
        }
    }
}
