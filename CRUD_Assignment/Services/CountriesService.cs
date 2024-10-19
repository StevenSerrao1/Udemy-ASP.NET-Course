using ServiceContracts;
using ServiceContracts.DTO;
using System.Runtime.CompilerServices;
using Entities;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
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
            // Convert "contryAddRequest" from "CountryAddRequest" to "Country" type
            Country country = countryAddRequest.ToCountry();
            
            // Generate a new CountryID (GUID)
            country.CountryId = Guid.NewGuid();

            // Check for duplicate in _countries
            if (_countries.Any(c => c.CountryName == countryAddRequest.CountryName))
            {
                throw new ArgumentException("Duplicate countries are not allowed");
            }

            // Add it into List<Country>
            _countries.Add(country);

            // Return CountryResponse object with generated CountryID (GUID)
            return country.ToCountryResponse();

        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(countries => countries.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryById(Guid? id)
        {
            // Check if "countryID" != null
            if (id == null) return null;

            // Get matching country from List<Country> based id
            Country? match = _countries.FirstOrDefault(c => c.CountryId == id);

            // Check to see if matching country is null
            if (match == null) return null;

            // Convert match from country to country response type
            CountryResponse? resMatch = match.ToCountryResponse();

            // return country response object
            return resMatch;
        }
    }
}
