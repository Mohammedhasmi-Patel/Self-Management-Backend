
namespace SelfManagement.Domain.Entities
{
    public class State
    {
        public Guid Id { get; set; }

        public Guid CountryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string StateCode { get; set; } = string.Empty;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        // Navigation
        public Country Country { get; set; } = null!;

        public ICollection<City> Cities { get; set; } = new List<City>();

    }
}
