﻿using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>Returuns the country object as country response</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all countries
        /// </summary>
        /// <returns>All countries from the list as an object of List<CountryResponse> type</CountryResponse></returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country based on its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A country of CountryResponse type</returns>
        Task<CountryResponse>? GetCountryById(Guid? id);

        /// <summary>
        /// Returns a country based on its name
        /// </summary>
        /// <param name="name">Name to be matched</param>
        /// <returns>A country of CountryResponse type or null</returns>
        Task<CountryResponse?> GetCountryByName(string? name);
        
    }
}
