using Microsoft.AspNetCore.Identity;
using SelfManagement.Application.DTO.Auth;
using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.Exceptions;
using SelfManagement.Application.RepositoryInterface;
using SelfManagement.Application.RepositoryInterface.Common;
using SelfManagement.Application.ServiceInterface;
using SelfManagement.Application.ServiceInterface.Auth;
using SelfManagement.Domain.Entities;
using static SelfManagement.Application.Exceptions.BadRequestException;

namespace SelfManagement.Application.Services.Auth
{
    public class OtpService : IOtpService
    {
        private readonly IEmailService _emailService;
        private readonly IOtpRepository _otpRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        private const int ExpiryMinutes = 10;
        private const int MaxAttempts = 5;

        public OtpService(IEmailService emailService,IOtpRepository otpRepository, UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            _emailService = emailService;
            _otpRepository = otpRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<GenerateOtpResponse> GenerateAndSendOtpAsync(string email, Guid? userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            var isUserExist = await GetUserByEmail(email);
            if (!isUserExist)
            {
                throw new NotFoundException("No account found with this email address.");
            }

            var oldOtps = await _otpRepository.GetAllOtpByEmailAsync(email);

            foreach (UserOtp otp in oldOtps)
            {
                otp.IsUsed = true;
            }

            int otpCode = GenerateSixDigitOtp();
            var userOtp = new UserOtp()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Otp = otpCode,
                ExpiresAt = DateTime.UtcNow.AddMinutes(ExpiryMinutes),
                CreatedAt = DateTime.UtcNow
            };

            await _otpRepository.AddAsync(userOtp);

            await _emailService.SendEmailAsync(
                  email,
                  "Your verification code",
                  $"Your OTP is <b>{otpCode}</b>. It expires in {ExpiryMinutes} minutes."
            );
            await _unitOfWork.SaveChangesAsync();
            return new GenerateOtpResponse()
            {
                Message = "Otp Generate Successfully.",
                Success = true
            };
        }

        public async Task<ApiResponse<object>> VerifyOtpAsync(string email, int otp)
        {
            await _unitOfWork.BeginTransactionAsync();

            var otpRecord = await _otpRepository.GetLatestOtpByEmailAsync(email);

            if(otpRecord is null)
            {
                throw new NotFoundException("No OTP found. Please request a new OTP.");
            }

            var applicationUser = await _userManager.FindByEmailAsync(email);


            if (otpRecord.IsUsed)
            {
                throw new ConflictException("OTP already used. Please request a new OTP.");
            }

            if (otpRecord.ExpiresAt <= DateTime.UtcNow)
            {
                throw new ConflictException("OTP expired. Please request a new OTP.");
            }

            if (otpRecord.AttemptCount >= MaxAttempts)
                throw new ConflictException("Too many attempts. Please request a new OTP.");

            if(otpRecord.AttemptCount >= MaxAttempts)
            {
                throw new ConflictException("Too many attempts. Please request a new OTP.");
            }

            if(otpRecord.Otp != otp)
            {
                otpRecord.AttemptCount++;
                await _otpRepository.SaveChangesAsync();
                throw new BadRequestException("Invalid OTP.");
            }

            otpRecord.IsUsed = true;
            otpRecord.VerifiedAt = DateTime.UtcNow;

            applicationUser.EmailVerifiedAt = DateTime.UtcNow;
            applicationUser.EmailConfirmed = true;

            var result = await _userManager.UpdateAsync(applicationUser);

            if (!result.Succeeded)
            {
                throw new InternalServerErrorExcdeption("Failed to update user.");
            }

            await _otpRepository.SaveChangesAsync();
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, "OTP verified successfully.");
        }

        private static int GenerateSixDigitOtp()
        {
           return Random.Shared.Next(100000, 999999);
        }

        private async Task<bool> GetUserByEmail(string email)
        {
            var res =  await _userManager.FindByEmailAsync(email);
            return res != null;
        }
    }
}
