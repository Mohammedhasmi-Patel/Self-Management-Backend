
namespace SelfManagement.Application.DTO.Locations.City
{
    public class CityResponse
    {
        public Guid? Id { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? StateId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

    }
}
