
using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Infrastructure.Database;
using SelfManagement.Domain.Entities;
using SelfManagement.Application.DTO.Locations.States;

namespace SelfManagement.Infrastructure.Repository.Location
{
    public class StateRepository : IStateRepository
    {
        private readonly ApplicationDbContext _context;

        public StateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StateResponse?> GetStateByIdAsync(Guid id)
        {
            State? res = await _context.States.FirstOrDefaultAsync(x => x.Id == id);
            return new StateResponse() {
                Id = res?.Id,
                CountryId = res?.CountryId,
                Latitude = res?.Latitude,
                Longitude = res?.Longitude,
                Name = res?.Name
            };
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
