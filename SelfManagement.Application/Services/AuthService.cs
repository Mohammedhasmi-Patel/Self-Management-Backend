using Microsoft.AspNetCore.Identity;
using SelfManagement.Application.DTO.Auth;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.Exceptions;
using SelfManagement.Application.ServiceInterface;
using SelfManagement.Domain.Entities;
using SelfManagement.Domain.Enum;
using static SelfManagement.Application.Exceptions.BadRequestException;


namespace SelfManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOtpService _otpService;

        public AuthService(UserManager<ApplicationUser> userManager, IOtpService otpService)
        {
            _userManager = userManager;
            _otpService = otpService;
        }
        public async Task<ApiResponse<object>> RegisterUser(RegisterUserRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if(existingUser is not null)
            {
                throw new ConflictException("Email already exists.");
            }
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,   
                Email = request.Email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var result = await _userManager.CreateAsync(user,request.Password);
            if (!result.Succeeded)
            {
                string firstMessage = result.Errors?.FirstOrDefault()?.Description ?? "Something went wrong";
                throw new BadRequestException(firstMessage);
            }

            await _userManager.AddToRoleAsync(user, nameof(ApplicationUserRole.User));

            await _otpService.GenerateAndSendOtpAsync(user.Email, user.Id);

            return ApiResponse<object>.SuccessResponse(null, "User registered successfully.");

        }

        
    }
}
