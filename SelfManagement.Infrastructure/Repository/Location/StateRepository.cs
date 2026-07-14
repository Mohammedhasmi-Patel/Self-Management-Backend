
using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.DTO.Locations.States;
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

        public async Task<List<StateListDropdown>> GetStatesByCountryIdAsync(Guid id)
        {
            return await _context.States
            .Where(x => x.CountryId == id)
            .OrderBy(x => x.Name)
            .Select(x => new StateListDropdown
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
        }

        public async Task<bool> IsStateExistAsync(Guid id)
        {
            return await _context.States.AnyAsync(s => s.Id == id);
        }
    }
}
