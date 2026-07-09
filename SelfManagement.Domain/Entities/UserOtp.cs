
namespace SelfManagement.Domain.Entities
{
    public class UserOtp
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; } // Null until user is created (optional)
        public string Email { get; set; } = string.Empty;
        public int Otp { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public int AttemptCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ApplicationUser? User { get; set; }


    }
}
