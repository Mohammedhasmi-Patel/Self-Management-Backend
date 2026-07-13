
using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Infrastructure.Database;

namespace SelfManagement.Infrastructure.Repository.Location
{
    public class StateRepository : IStateRepository
    {
        private readonly ApplicationDbContext _context;

        public StateRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsStateExistAsync(Guid id)
        {
            return await _context.States.AnyAsync(s => s.Id == id);
        }
    }
}
