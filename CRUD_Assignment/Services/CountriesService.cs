using ServiceContracts;
using ServiceContracts.DTO;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        // Private fields
        private readonly ICountriesRepository _countriesRepository;

        // Constructor
        public CountriesService(ICountriesRepository countriesRepo)
        {
            _countriesRepository = countriesRepo;
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

            // Check for duplicate in _countriesRepository
            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Duplicate countries are not allowed");
            }

            // Convert "contryAddRequest" from "CountryAddRequest" to "Country" type
            Country country = countryAddRequest.ToCountry();
            
            // Generate a new CountryID (GUID)
            country.CountryId = Guid.NewGuid();

            // Add it into List<Country>
            await _countriesRepository.AddCountry(country);

           // Return CountryResponse object with generated CountryID (GUID)
           return country.ToCountryResponse();

        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            List<CountryResponse> countries = (await _countriesRepository.GetAllCountries())
                .Select(countries => countries.ToCountryResponse())
                .ToList();

            Console.WriteLine();

            return countries;
        }

        public async Task<CountryResponse>? GetCountryById(Guid? id)
        {
            // Check if "countryID" != null
            if (id == null) return null!;

            // Get matching country from List<Country> based id
            Country? match = await _countriesRepository.GetCountryByCountryId(id.Value);

            // Check to see if matching country is null
            if (match == null) return null!;

            // Convert match from country to country response type
            CountryResponse? resMatch = match.ToCountryResponse();

            // return country response object
            return resMatch;
        }

        public async Task<CountryResponse?> GetCountryByName(string? name)
        {
            // Check if "countryID" != null
            if (name == null) return null!;

            // Get matching country from List<Country> based on name
            Country? match = await _countriesRepository.GetCountryByCountryName(name);

            // Check to see if matching country is null
            if (match == null) return null!;

            // Convert match from country to country response type
            CountryResponse? resMatch = match.ToCountryResponse();

            // return country response object
            return resMatch;
        }
    }
}
