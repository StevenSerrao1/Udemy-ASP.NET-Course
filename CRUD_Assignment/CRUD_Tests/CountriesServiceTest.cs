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
using RepositoryContracts;

namespace CRUD_Tests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture;
        private readonly ICountriesRepository _countriesRepository;
        private readonly Mock<ICountriesRepository> _countriesRepoMock;

        public CountriesServiceTest()
        {
            // Initialize fixture for mock data
            _fixture = new Fixture();
            _countriesRepoMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepoMock.Object;

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

            _countriesService = new CountriesService(_countriesRepository);
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
        public async Task AddCountry_DuplicateCountryName_ShouldThrowException()
        {
            // Arrange / Make two countries with identical names
            CountryAddRequest countryAR =
                _fixture.Build<CountryAddRequest>()
                .With(c => c.CountryName, "U.S.A")
                .Create();

            // Create mock implementation for adding country
            _countriesRepoMock
                .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(countryAR.ToCountry());

            var duplicateCountry = new Country { CountryName = "U.S.A" };

            // Mock GetCountryByCountryName to return the duplicate country
            _countriesRepoMock
                .Setup(temp => temp.GetCountryByCountryName("U.S.A"))
                .ReturnsAsync(duplicateCountry);

            // Act & Assert
            // The exception should be thrown, and we should catch it
            await FluentActions
                .Invoking(async () => await _countriesService.AddCountry(countryAR))
                .Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Duplicate countries are not allowed");
        }


        // If approved country name is supplied, add the country name into Countries list
        [Fact]
        public async Task AddCountry_ProperCountryDetails_ToSucceed()
        {
            // Arrange / Make a country with a null valued name
            CountryAddRequest request = _fixture
                .Build<CountryAddRequest>()
                .Create();

            Country country = request.ToCountry();
            CountryResponse countryResponse = country.ToCountryResponse();

            // Create mock implementation
            _countriesRepoMock
                .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country);

            Country duplicateCountry = new Country { CountryName = "U.S.A" };

            // Mock GetCountryByCountryName to return the duplicate country
            _countriesRepoMock
                .Setup(temp => temp.GetCountryByCountryName("U.S.A"))
                .ReturnsAsync(duplicateCountry);

            // Act
            CountryResponse response = await _countriesService.AddCountry(request);
            countryResponse.CountryID = response.CountryID;

            Console.WriteLine();

            // Assert
            //Assert.True(response.CountryID != Guid.Empty);
            response.CountryID.Should().NotBeEmpty(); // FLUENT ASSERTION
            //Assert.Contains(response, countries_from_GetAllCountries);
            response.Should().Be(countryResponse);
        }
        #endregion

        #region GetAllCountries() Test Cases

        // List should be empty by default
        [Fact]
        public async Task GetAllCountries_EmptyList_ShouldBeEmpty()
        {
            List<Country> countries = new List<Country>();
            _countriesRepoMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);

            // Act 
            List<CountryResponse> countriesList = await _countriesService.GetAllCountries();

            // Assert
            //Assert.Empty(countriesList);
            countriesList.Should().BeEmpty(); // FLUENT ASSERTION
        }

        // If countries are added, the countires should be returned
        [Fact]
        public async Task GetAllCountries_ShouldHaveFewCountries()
        {
            //Arrange
            List<Country> country_list = new List<Country>() {
                _fixture.Build<Country>()
                .With(temp => temp.Persons, null as List<Person>).Create(),
                _fixture.Build<Country>()
                .With(temp => temp.Persons, null as List<Person>).Create()
            };

            List<CountryResponse> country_response_list = country_list.Select(temp => temp.ToCountryResponse()).ToList();

            _countriesRepoMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(country_list);

            //Act
            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            //Assert
            actualCountryResponseList.Should().BeEquivalentTo(country_response_list);
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
