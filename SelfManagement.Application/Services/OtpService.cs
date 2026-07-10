using SelfManagement.Application.DTO.Common;
using SelfManagement.Application.Exceptions;
using SelfManagement.Application.RepositoryInterface;
using SelfManagement.Application.ServiceInterface;
using SelfManagement.Domain.Entities;
using static SelfManagement.Application.Exceptions.BadRequestException;

namespace SelfManagement.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly IEmailService _emailService;
        private readonly IOtpRepository _otpRepository;
        private const int ExpiryMinutes = 10;
        private const int MaxAttempts = 5;

        public OtpService(IEmailService emailService,IOtpRepository otpRepository)
        {
            _emailService = emailService;
            _otpRepository = otpRepository;
        }
        public async Task<int> GenerateAndSendOtpAsync(string email, Guid? userId)
        {
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

            return otpCode;
        }

        public async Task<ApiResponse<object>> VerifyOtpAsync(string email, int otp)
        {
            var otpRecord = await _otpRepository.GetLatestOtpByEmailAsync(email);

            if(otpRecord is null)
            {
                throw new NotFoundException("No OTP found. Please request a new OTP.");
            }

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
            await _otpRepository.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, "OTP verified successfully.");
        }

        private static int GenerateSixDigitOtp()
        {
           return Random.Shared.Next(100000, 999999);
        }
    }
}
