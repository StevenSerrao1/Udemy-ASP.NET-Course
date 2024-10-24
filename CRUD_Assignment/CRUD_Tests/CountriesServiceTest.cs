using ServiceContracts;
using Entities;
using System;
using Services;
using ServiceContracts.DTO;

namespace CRUD_Tests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }

        #region AddCountry() Test Cases
        // If CountryAddRequest is null, it should throw null arg exceptions
        [Fact]
        public void AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? country = null;

            // Act
            // Is within the assert method

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _countriesService.AddCountry(country);
            });
        }

        // If countryname is null, throw argument exception
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            // Arrange / Make a country with a null valued name
            CountryAddRequest country = new CountryAddRequest() { CountryName = null };

            // Assert / Throw an excpetion based on that
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act / use method that will return said null value
                _countriesService.AddCountry(country);
            });
        }

        // If country name is duplicate, throw argument exception
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            // Arrange / Make two countries with identical names
            CountryAddRequest country1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country2 = new CountryAddRequest() { CountryName = "USA" };

            // Assert / Throw an excpetion based on that
            Assert.Throws<ArgumentException>(() =>
            {
                // Act / use method that will err when duplicate country names are used
                _countriesService.AddCountry(country1);
                _countriesService.AddCountry(country2);
            });
        }

        // If approved country name is supplied, add the country name into Countries list
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            // Arrange / Make a country with a null valued name
            CountryAddRequest request = new CountryAddRequest() { CountryName = "RSA" };

            // Act
            CountryResponse response = _countriesService.AddCountry(request);

            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            // Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }
        #endregion

        #region GetAllCountries() Test Cases

        // List should be empty by default
        [Fact]
        public void GetAllCountries_EmptyList()
        {

            // Act 
            List<CountryResponse> countriesList = _countriesService.GetAllCountries();

            // Assert
            Assert.Empty(countriesList);

        }

        // If countries are added, the countires should be returned
        [Fact]
        public void GetAllCountries_AddCountries()
        {
            // Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "USA" },
                new CountryAddRequest() { CountryName = "RSA" },
                new CountryAddRequest() { CountryName = "FRA" },
            };

            List<CountryResponse> countries_response_list = new List<CountryResponse>();

            // Act

            // for each country_request in request list, add it to response list
            foreach (CountryAddRequest country_request in country_request_list)
            {
                countries_response_list.Add(_countriesService.AddCountry(country_request));
            }

            // create actual response list, gathering country responses using method
            List<CountryResponse> actualResponseList = _countriesService.GetAllCountries();

            // Read each element from list
            foreach(CountryResponse expected_country in countries_response_list)
            {
                // Assert
                Assert.Contains(expected_country, actualResponseList);
            }
        }
        #endregion

        #region GetCountryById() Test Cases

        [Fact]
        public void GetCountryById_ValidId()
        {
            // Arrange
            // Create new CountryAddRequest
            CountryAddRequest country = new CountryAddRequest()
            { 
                CountryName = "USA" 
            };

            // Use .AddCountry() to assign into CountryResponse variable
            CountryResponse returned_country = _countriesService.AddCountry(country);

            // Act
            CountryResponse? actualCountry = _countriesService.GetCountryById(returned_country.CountryID);

            // Assert
            if (actualCountry != null)
            {
                Assert.Equal(returned_country.CountryID, actualCountry.CountryID);
            }
            
        }

        [Fact]
        public void GetCountryById_NullId()
        {
            // Arrange
            Guid? country = null;

            // Act
            CountryResponse? countryNull = _countriesService.GetCountryById(country);

            // Assert
            Assert.Null(countryNull);          
        }


        #endregion


    }
}
