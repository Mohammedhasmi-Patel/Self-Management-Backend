
using Microsoft.AspNetCore.Mvc;
using SelfManagement.Application.DTO.Auth;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.ServiceInterface.Auth;

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
            var response = await _otpService.VerifyOtpAsync(request.Email, request.Otp);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var response = await _authService.LoginUser(request);
            return Ok(response);
        }

        [HttpPost("generate-otp")]
        public async Task<IActionResult> GenerateAndSendOtpAsync(string email)
        {
            var response = await _otpService.GenerateAndSendOtpAsync(email,null);
            var res = ApiResponse<object>.SuccessResponse(null, "We have sent otp to your email");
            return Ok(response);
        }
    }
}
