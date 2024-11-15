using ServiceContracts;
using Entities;
using System;
using Services;
using ServiceContracts.DTO;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;

namespace CRUD_Tests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            // Init empty countries list
            var countriesMockData = new List<Country>() { };

            // Create mock version of dbContext 
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
            );

            // Assign object of dbContextMock into an ApplicationDbContext object
            ApplicationDbContext dbContext = dbContextMock.Object;

            // Create Mock DbSet of countries type
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesMockData);

            _countriesService = new CountriesService(dbContext);
        }

        #region AddCountry() Test Cases
        // If CountryAddRequest is null, it should throw null arg exceptions
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? country = null;

            // Act
            // Is within the assert method

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(country);
            });
        }

        // If countryname is null, throw argument exception
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            // Arrange / Make a country with a null valued name
            CountryAddRequest country = new CountryAddRequest() { CountryName = null };

            // Assert / Throw an excpetion based on that
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act / use method that will return said null value
                await _countriesService.AddCountry(country);
            });
        }

        // If country name is duplicate, throw argument exception
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            // Arrange / Make two countries with identical names
            CountryAddRequest country1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country2 = new CountryAddRequest() { CountryName = "USA" };

            await _countriesService.AddCountry(country1);

            // Assert / Throw an excpetion based on that
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act / use method that will err when duplicate country names are used
                await _countriesService.AddCountry(country2);
            });
        }

        // If approved country name is supplied, add the country name into Countries list
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            // Arrange / Make a country with a null valued name
            CountryAddRequest request = new CountryAddRequest() { CountryName = "RSA" };

            // Act
            CountryResponse response = await _countriesService.AddCountry(request);

            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();

            // Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }
        #endregion

        #region GetAllCountries() Test Cases

        // List should be empty by default
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {

            // Act 
            List<CountryResponse> countriesList = await _countriesService.GetAllCountries();

            // Assert
            Assert.Empty(countriesList);

        }

        // If countries are added, the countires should be returned
        [Fact]
        public async Task GetAllCountries_AddCountries()
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
                countries_response_list.Add(await _countriesService.AddCountry(country_request));
            }

            // create actual response list, gathering country responses using method
            List<CountryResponse> actualResponseList = await _countriesService.GetAllCountries();

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
        public async Task GetCountryById_ValidId()
        {
            // Arrange
            // Create new CountryAddRequest
            CountryAddRequest country = new CountryAddRequest()
            { 
                CountryName = "USA" 
            };

            // Use .AddCountry() to assign into CountryResponse variable
            CountryResponse returned_country = await _countriesService.AddCountry(country);

            // Act
            CountryResponse? actualCountry = await _countriesService.GetCountryById(returned_country.CountryID)!;

            // Assert
            if (actualCountry != null)
            {
                Assert.Equal(returned_country.CountryID, actualCountry.CountryID);
            }
            
        }

        [Fact]
        public async Task GetCountryById_NullId()
        {
            // Arrange
            Guid? country = null;

            // Act
            CountryResponse? countryNull = await _countriesService.GetCountryById(country)!;

            // Assert
            Assert.Null(countryNull);          
        }


        #endregion


    }
}
