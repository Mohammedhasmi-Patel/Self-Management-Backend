using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.RepositoryInterface;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;

namespace SelfManagement.Infrastructure.Repository
{
    public class OtpRepository : IOtpRepository
    {
        private readonly ApplicationDbContext _context;

        public OtpRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserOtp userOtp)
        {
            _context.UserOtps.Add(userOtp);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<UserOtp>> GetActiveOtpsByEmailAsync(string email)
        {
            return await _context.UserOtps
                    .Where(o => o.Email == email && !o.IsUsed )
                    .ToListAsync();
        }

        public async Task<ICollection<UserOtp>> GetAllOtpByEmailAsync(string email)
        {
            List<UserOtp> otps = await _context.UserOtps.Where(x => x.Email == email ).ToListAsync();
            return otps;
        }

        public async Task<UserOtp?> GetLatestOtpByEmailAsync(string email)
        {
            var em = email;
            UserOtp? latestRecord = await _context.UserOtps
                            .Where(x => x.Email == email && !x.IsUsed)
                            .OrderByDescending(x => x.CreatedAt)
                            .FirstOrDefaultAsync();

            if (latestRecord == null)
            {
                return null;
            }

            return latestRecord;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
