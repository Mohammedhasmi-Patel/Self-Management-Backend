
namespace SelfManagement.Domain.Entities
{
    public class City
    {
        public Guid Id { get; set; }

        public Guid CountryId { get; set; }

        public Guid StateId { get; set; }


        public string Name { get; set; } = string.Empty;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public Country Country { get; set; } = null!;

        public State State { get; set; } = null!;

    }
}
