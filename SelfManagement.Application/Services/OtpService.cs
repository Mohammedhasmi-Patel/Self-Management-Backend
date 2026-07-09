using SelfManagement.Application.RepositoryInterface;
using SelfManagement.Application.ServiceInterface;
using SelfManagement.Domain.Entities;

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

        public async Task<(bool Success, string Message)> VerifyOtpAsync(string email, int otp)
        {
            var latestOtpRecord = await _otpRepository.GetLatestOtpByEmailAsync(email);
            if (latestOtpRecord is null)
            {
                return (false, "No OTP found. Please request a new one.");
            }

            if (latestOtpRecord.IsUsed)
            {
                return (false, "OTP already used. Please request a new one.");
            }

            if (latestOtpRecord.ExpiresAt < DateTime.UtcNow)
            {
                return (false, "OTP expired. Please request a new one.");
            }

            if(latestOtpRecord.AttemptCount >= MaxAttempts)
            {
                return (false, "Too many attempts. Please request a new OTP.");
            }

            latestOtpRecord.AttemptCount++;

            if (latestOtpRecord.Otp != otp)
            {
                await _otpRepository.SaveChangesAsync();
                return (false, "Invalid OTP.");
            }

            latestOtpRecord.IsUsed = true;
            latestOtpRecord.VerifiedAt = DateTime.UtcNow;
            await _otpRepository.SaveChangesAsync();
            return (true, "OTP verified successfully.");

        }

        private static int GenerateSixDigitOtp()
        {
           return Random.Shared.Next(100000, 999999);
        }
    }
}
