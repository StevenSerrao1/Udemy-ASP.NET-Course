using ServiceContracts;
using ServiceContracts.DTO;
using System.Runtime.CompilerServices;
using Entities;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        // Private fields
        private readonly PersonsDbContext _db;

        // Constructor
        public CountriesService(PersonsDbContext dbContext)
        {
            _db = dbContext;
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {

            // Check if "countryAddRequest is not null"
            if (countryAddRequest == null)
            {    
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            // Validate all properties of "countryAddRequest"
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest.CountryName));
            }

            // Check for duplicate in _db
            if (_db.Countries.Count(c => c.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Duplicate countries are not allowed");
            }

            // Convert "contryAddRequest" from "CountryAddRequest" to "Country" type
            Country country = countryAddRequest.ToCountry();
            
            // Generate a new CountryID (GUID)
            country.CountryId = Guid.NewGuid();      

            // Add it into List<Country>
            _db.Countries.Add(country);

            // Save changes - NEW
            _db.SaveChanges();

            // Return CountryResponse object with generated CountryID (GUID)
            return country.ToCountryResponse();

        }

        public List<CountryResponse> AddMockCountryResponses()
        {
            List<CountryResponse> countryResponseList = new List<CountryResponse>();
            List<CountryAddRequest> countryARList = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "Canada" },
                new CountryAddRequest() { CountryName = "India" },
                new CountryAddRequest() { CountryName = "U.S.A" },
            };

            foreach(CountryAddRequest countryAddRequest in countryARList)
            {
                countryResponseList.Add(AddCountry(countryAddRequest));
            }

            return countryResponseList;
        }

        public List<Country> AddMockCountries()
        {
            List<Country> countries = new List<Country>()
            {
                new Country { CountryName = "U.S.A", 
                    CountryId = Guid.Parse("02CA2B03-63BD-4891-9A59-E92436CA0F33") },

                new Country { CountryName = "Germany",
                    CountryId = Guid.Parse("641670A5-FED6-44E9-AB25-E3B18C6DC7C8") },

                new Country { CountryName = "Australia",
                    CountryId = Guid.Parse("2B061FD2-F54C-4FF6-A77F-3036827F53D3") },

                new Country { CountryName = "South-Africa",
                    CountryId = Guid.Parse("EE878C05-7FE6-4F23-8C44-A4CD10D410A2") }
            };

            return countries;
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _db.Countries.Select(countries => countries.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryById(Guid? id)
        {
            // Check if "countryID" != null
            if (id == null) return null;

            // Get matching country from List<Country> based id
            Country? match = _db.Countries.FirstOrDefault(c => c.CountryId == id);

            // Check to see if matching country is null
            if (match == null) return null;

            // Convert match from country to country response type
            CountryResponse? resMatch = match.ToCountryResponse();

            // return country response object
            return resMatch;
        }
    }
}
