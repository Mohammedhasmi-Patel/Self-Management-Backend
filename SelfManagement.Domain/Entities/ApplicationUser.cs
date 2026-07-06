

namespace SelfManagement.Domain.Entities
{
    public class ApplicationUser 
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName =>
            $"{FirstName} {LastName}".Trim();
        public string? ProfileImageUrl { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime? EmailVerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
