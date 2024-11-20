using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents Data Access Logic for managing Country entity
    /// </summary>
    public interface ICountriesRepository
    {

        /// <summary>
        /// Adds a new Country object to data store
        /// </summary>
        /// <param name="country">Country object to be added</param>
        /// <returns>Returns Country object after being added to data store</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Returns a List<Country> of all countries in data store
        /// </summary>
        /// <returns>List<Country></returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Returns an object of Country type based on countryId (Guid)
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>Object of Country type or null</returns>
        Task<Country?> GetCountryByCountryId(Guid countryId);

        /// <summary>
        /// Retrieve Country object based on countryName
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns>Object of Country type or null</returns>
        Task<Country?> GetCountryByCountryName(string? countryName);
    }
}
