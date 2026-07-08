using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.DTO.Auth;
using SelfManagement.Application.ServiceInterface;

namespace SelfManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var result = await _authService.RegisterUser(request);
            return Ok(result);
        }
    }
}
