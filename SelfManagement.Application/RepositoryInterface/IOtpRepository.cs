
using SelfManagement.Domain.Entities;

namespace SelfManagement.Application.RepositoryInterface
{
    public interface IOtpRepository
    {
        public Task<ICollection<UserOtp>> GetAllOtpByEmailAsync(string email);
        public Task AddAsync(UserOtp userOtp);
        public Task<UserOtp?> GetLatestOtpByEmailAsync(string email);
        Task SaveChangesAsync();
    }
}
