using ServiceContracts;
using Entities;
using System;
using Services;
using ServiceContracts.DTO;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;
using FluentAssertions;

namespace CRUD_Tests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture;

        public CountriesServiceTest()
        {
            // Initialize fixture for mock data
            _fixture = new Fixture();

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
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    // Act
            //    await _countriesService.AddCountry(country);
            //});

            Func<Task> action = async () => await _countriesService.AddCountry(country);

            await action.Should().ThrowAsync<ArgumentNullException>(); // FLUENT ASSERTION 
        }

        // If countryname is null, throw argument exception
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            // Arrange / Make a country with a null valued name
            CountryAddRequest country = _fixture.Build<CountryAddRequest>()
                .With(c => c.CountryName, null as string)
                .Create();

            // Assert / Throw an excpetion based on that
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    // Act / use method that will return said null value
            //    await _countriesService.AddCountry(country);
            //});

            Func<Task> action = async() => await _countriesService.AddCountry(country);

            await action.Should().ThrowAsync<ArgumentNullException>();
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
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    // Act / use method that will err when duplicate country names are used
            //    await _countriesService.AddCountry(country2);
            //});

            Func<Task> action = async () => await _countriesService.AddCountry(country2);

            await action.Should().ThrowAsync<ArgumentException>();

        }

        // If approved country name is supplied, add the country name into Countries list
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            // Arrange / Make a country with a null valued name
            CountryAddRequest request = _fixture.Create<CountryAddRequest>();

            // Act
            CountryResponse response = await _countriesService.AddCountry(request);

            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();

            // Assert
            //Assert.True(response.CountryID != Guid.Empty);
            response.CountryID.Should().NotBeEmpty(); // FLUENT ASSERTION
            //Assert.Contains(response, countries_from_GetAllCountries);
            countries_from_GetAllCountries.Should().Contain(response); // FLUENT ASSERTION
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
            //Assert.Empty(countriesList);
            countriesList.Should().BeEmpty(); // FLUENT ASSERTION
        }

        // If countries are added, the countires should be returned
        [Fact]
        public async Task GetAllCountries_AddCountries()
        {
            // Arrange
            List<CountryAddRequest> country_request_list = _fixture.CreateMany<CountryAddRequest>(3).ToList();

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
                // Assert.Contains(expected_country, actualResponseList);
                actualResponseList.Should().Contain(expected_country); // FLUENT ASSERTION
            }
        }
        #endregion

        #region GetCountryById() Test Cases

        [Fact]
        public async Task GetCountryById_ValidId()
        {
            // Arrange
            // Create new CountryAddRequest
            CountryAddRequest country = _fixture.Create<CountryAddRequest>();

            // Use .AddCountry() to assign into CountryResponse variable
            CountryResponse returned_country = await _countriesService.AddCountry(country);

            // Act
            CountryResponse? actualCountry = await _countriesService.GetCountryById(returned_country.CountryID)!;

            // Assert
            if (actualCountry != null)
            {
                // Assert.Equal(returned_country.CountryID, actualCountry.CountryID);
                actualCountry.CountryID.Should().Be(returned_country.CountryID); // FLUENT ASSERTION
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
            // Assert.Null(countryNull);
            countryNull.Should().BeNull();
        }

        #endregion

    }
}
