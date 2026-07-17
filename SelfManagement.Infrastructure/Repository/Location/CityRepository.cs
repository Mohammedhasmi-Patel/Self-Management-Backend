

using Microsoft.EntityFrameworkCore;
using SelfManagement.Application.DTO.Locations.City;
using SelfManagement.Application.RepositoryInterface.Locations;
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;

namespace SelfManagement.Infrastructure.Repository.Location
{
    public class CityRepository : ICitiesRepository
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
                .OrderBy(x  => x.Name)
                .Select(x => new CityListDropdown()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
        }

        public async Task<CityResponse?> GetCityByIdAsync(Guid id)
        {
           City city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (city is null) return null;
            return new CityResponse()
            {
                Id = city.Id,
                StateId = city.StateId,
                CountryId = city.CountryId,
                Name = city.Name,
                Latitude = city.Latitude,
                Longitude = city.Longitude
            };
        }

        public async Task<bool> IsCityExistAsync(Guid id)
        {
            return await _context.Cities.AnyAsync(c => c.Id == id);
        }
    }
}
