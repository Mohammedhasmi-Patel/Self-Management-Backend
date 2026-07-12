
using SelfManagement.Domain.Entities;
using SelfManagement.Infrastructure.Database;
using SelfManagement.Infrastructure.Seeder.JsonDto;
using System.Globalization;
using System.Text.Json;

namespace SelfManagement.Infrastructure.Seeder
{
    public class CountryStateCitySeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Countries.Any()) return;
            string path = "E:\\FullstackProjects\\self-management-backend\\SelfManagementSolution\\SelfManagement.Infrastructure\\Seeder\\JsonFiles\\countries+states+cities.json";
            var json = await File.ReadAllTextAsync(path);
            var jsonCountries = JsonSerializer.Deserialize<List<CountryJson>>(json,new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            if (jsonCountries == null) return;
            var countries = new List<Country>();
            var states = new List<State>();
            var cities = new List<City>();


            foreach (var countryJson in jsonCountries)
            {
                var country = new Country
                {
                    Id = Guid.NewGuid(),
                    Name = countryJson.Name,
                    Iso2 = countryJson.Iso2,
                    Iso3 = countryJson.Iso3,
                    NumericCode = countryJson.NumericCode,
                    PhoneCode = countryJson.PhoneCode,
                    Capital = countryJson.Capital,
                    Currency = countryJson.Currency,
                    CurrencyName = countryJson.CurrencyName,
                    CurrencySymbol = countryJson.CurrencySymbol,
                    Native = countryJson.Native,
                    Region = countryJson.Region,
                    Subregion = countryJson.Subregion,
                    Emoji = countryJson.Emoji,
                    EmojiU = countryJson.EmojiU,
                    Latitude = ParseDecimal(countryJson.Latitude),
                    Longitude = ParseDecimal(countryJson.Longitude)
                };

                countries.Add(country);
                foreach (var stateJson in countryJson.States)
                {
                    var state = new State
                    {
                        Id = Guid.NewGuid(),
                        CountryId = country.Id,
                        Name = stateJson.Name,
                        StateCode = stateJson.StateCode,
                        Latitude = ParseDecimal(stateJson.Latitude),
                        Longitude = ParseDecimal(stateJson.Longitude)
                    };

                    states.Add(state);

                    foreach (var cityJson in stateJson.Cities)
                    {
                        cities.Add(new City
                        {
                            Id = Guid.NewGuid(),
                            CountryId = country.Id,
                            StateId = state.Id,
                            Name = cityJson.Name,
                            Latitude = ParseDecimal(cityJson.Latitude),
                            Longitude = ParseDecimal(cityJson.Longitude)
                        });
                    }
                }
            }

            context.Countries.AddRange(countries);
            context.States.AddRange(states);
            context.Cities.AddRange(cities);

            await context.SaveChangesAsync();

        }
        private static decimal? ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return decimal.TryParse(
                value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var result)
                ? result
                : null;
        }

    }
}
