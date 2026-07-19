using Microsoft.AspNetCore.Identity;
using SelfManagement.Application.DTO.Auth;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.Exceptions;
using SelfManagement.Application.RepositoryInterface.Common;
using SelfManagement.Application.ServiceInterface.Auth;
using SelfManagement.Domain.Entities;
using SelfManagement.Domain.Enum;


namespace SelfManagement.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOtpService _otpService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(UserManager<ApplicationUser> userManager, IOtpService otpService, IJwtService jwtService,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _otpService = otpService;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<LoginUserResponse>> LoginUser(LoginUserRequest request)
        {
            string email = request.Email;
            
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser is null)
            {
                throw new NotFoundException("User not found.");
            }

            bool isPasswordValid = await _userManager.CheckPasswordAsync(existingUser,request.Password);

            if (!isPasswordValid)
            {
                throw new UnauthorizedException("Invalid credentials");
            }

            JwtConfigurationResponse tokenResponse = await _jwtService.GenerateJwtToken(existingUser!);
            if (!tokenResponse.Success)
            {
                throw new InternalServerErrorExcdeption("Something went wrong.");
            }

            LoginUserResponse response = new LoginUserResponse()
            {
                Token = tokenResponse.Token,
                Email = email,
                ProfileUrl = existingUser?.ProfileImageUrl,
                FullName = existingUser.FullName,
                ExpiresAt = tokenResponse.ExpiredAt
                
            };

            return ApiResponse<LoginUserResponse>.SuccessResponse(response,"User loggedin successfully.");
        }

        public async Task<ApiResponse<object>> RegisterUser(RegisterUserRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
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
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            return ApiResponse<object>.SuccessResponse(null, "User registered successfully.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
