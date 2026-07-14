

using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.DTO.Locations.City;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Infrastructure.Database;

namespace SelfManagement.Infrastructure.Repository.Location
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _context;

        public CityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CityListDropdown>> GetCitiesByStateIdAsync(Guid id)
        {
           return await _context.Cities
                .Where(x => x.StateId == id)
                .Select(x => new CityListDropdown()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
        }

        public async Task<bool> IsCityExistAsync(Guid id)
        {
            return await _context.Cities.AnyAsync(c => c.Id == id);
        }
    }
}
