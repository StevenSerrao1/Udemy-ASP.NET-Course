using System;
using Entities;

namespace ServiceContracts.DTO
{

    /// <summary>
    /// DTO class for providing country details as a response
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }

        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CountryResponse)) return false;

            CountryResponse country_to_compare = (CountryResponse)obj;

            return this.CountryName == country_to_compare.CountryName && this.CountryID == country_to_compare.CountryID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtensions
    {
        // This method takes the country object from CountryAddRequest and returns 
        // a CountryResponse object with an additional property, the countryID
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse() 
            { 
                CountryID = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}
