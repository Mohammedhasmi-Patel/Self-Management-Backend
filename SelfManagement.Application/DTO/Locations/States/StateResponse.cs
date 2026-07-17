
namespace SelfManagement.Application.DTO.Locations.States
{
    public class StateResponse
    {
        public Guid? Id { get; set; }
        public Guid? CountryId { get; set; }
        public string? Name { get; set; } = string.Empty;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
    }
}
