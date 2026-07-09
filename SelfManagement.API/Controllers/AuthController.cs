using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.DTO.Auth;
using SelfManagement.Application.Exceptions;
using SelfManagement.Application.ServiceInterface;
using static System.Net.WebRequestMethods;

namespace SelfManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOtpService _otpService;

        public AuthController(IAuthService authService,IOtpService otpService)
        {
            _authService = authService;
            _otpService = otpService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var result = await _authService.RegisterUser(request);
            return Ok(result);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
        {
            var (success, message) = await _otpService.VerifyOtpAsync(request.Email, request.Otp);
            if (!success)
            {
                throw new BadRequestException(message);
            }

            return Ok(message);
        }
    }
}
