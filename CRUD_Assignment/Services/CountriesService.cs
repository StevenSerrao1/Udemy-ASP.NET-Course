using ServiceContracts;
using ServiceContracts.DTO;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
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

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
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
            if (await _db.Countries.CountAsync(c => c.CountryName == countryAddRequest.CountryName) > 0)
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
            await _db.SaveChangesAsync();

           // Return CountryResponse object with generated CountryID (GUID)
           return country.ToCountryResponse();

        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _db.Countries.Select(countries => countries.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse>? GetCountryById(Guid? id)
        {
            // Check if "countryID" != null
            if (id == null) return null!;

            // Get matching country from List<Country> based id
            Country? match = await _db.Countries.FirstOrDefaultAsync(c => c.CountryId == id);

            // Check to see if matching country is null
            if (match == null) return null!;

            // Convert match from country to country response type
            CountryResponse? resMatch = match.ToCountryResponse();

            // return country response object
            return resMatch;
        }

    }
}
