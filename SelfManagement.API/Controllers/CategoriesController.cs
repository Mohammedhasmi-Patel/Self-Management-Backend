using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.ServiceInterface.Category;
using SelfManagement.Application.DTO.Categories;

namespace SelfManagement.API.Controllers
{
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync([FromQuery] GetCategoriesRequest request)
        {
            var response = await _categoriesService.GetAllCategoriesAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var response = await _categoriesService.CreateCategoryAsync(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            var response = await _categoriesService.UpdateCategoryAsync(request);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryAsync(Guid id)
        {
            var response = await _categoriesService.DeleteCategoryByIdAsync(id);
            return StatusCode(201, response);
        }
    }
}
