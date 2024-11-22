using AutoFixture;
using Moq;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Example.Controllers;
using ServiceContracts.DTO;
using FluentAssertions;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD_Tests
{
    public class PersonsControllerTests
    {
        // Initialize service fields
        private readonly ICountriesService _countriesService;
        private readonly IPersonService _personsService;

        // Init mock fields
        private readonly Mock<IPersonService> _personMock;
        private readonly Mock<ICountriesService> _countryMock;

        // Init fixture field
        private readonly Fixture _fixture;

        public PersonsControllerTests()
        {
            // Instantiate fixture
            _fixture = new Fixture();

            // Instantiate mock objects
            _personMock = new Mock<IPersonService>();
            _countryMock = new Mock<ICountriesService>();

            // Instantiate mock objects INTO service objects
            _countriesService = _countryMock.Object;
            _personsService = _personMock.Object;
        }

        #region Index Action Tests

        [Fact]
        public async Task Index_ReturnIndexViewWithPersonsList()
        {
            // Arrange // Create list of PersonResponse type
            List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();
            // Create object of PersonController type (!)
            PersonsController personsController = new PersonsController(_personsService, _countriesService);

            // Create MOCK methods // GetFilteredPersons() mock method
            _personMock
                .Setup(temp => temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(persons_response_list);
            // GetSortedPersons() mock method
            _personMock
                .Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderEnum>()))
                .ReturnsAsync(persons_response_list);

            // Act // use AutoFixture to create dummy values for the constructor
            IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderEnum>());

            // Assert // Assert that the result returns a view/is of viewresult type
            // Shallow way of doing it // result.Should().BeOfType<ViewResult>();
            // Deeper way of doing it // Assign object casted to Type T if succesful
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            // FluentAssertion to verify that data can be assigned to List<PersonResponse>()
            viewResult.ViewData.Model.Should().BeAssignableTo<List<PersonResponse>>();

            // Final direct assertion
            viewResult.ViewData.Model.Should().Be(persons_response_list);
        }

        #endregion

        #region Create Action Tests

        [Fact]
        public async Task Create_GetRequest()
        {
            // Arrange // Create list of countryresponse type
            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();
            // Create object of PersonController type (!)
            PersonsController personsController = new PersonsController(_personsService, _countriesService);
            // Create Mock method for GetAllCountries()
            _countryMock
                .Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            // Act
            IActionResult action = await personsController.Create();

            // Assert
            ViewResult result = Assert.IsType<ViewResult>(action);
            var selectLisItems = Assert.IsAssignableFrom<List<SelectListItem>>(result.ViewData["Countries"]);    
        }

        [Fact]
        public async Task Create_PostRequest_InvalidModelState()
        {
            // Arrange // Initialize PAR due to parameter of controller action method
            PersonAddRequest personAdd = _fixture.Create<PersonAddRequest>();

            // Initialize PR to be returned by mock method of AddPerson()
            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            // Create list of countryresponse type
            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            // Create object of PersonController type (!)
            PersonsController personsController = new PersonsController(_personsService, _countriesService);

            // Create Mock method for GetAllCountries()
            _countryMock
                .Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            // Create Mock method for GetAllCountries()
            _personMock
                .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponse);

            // *** ADD MODEL ERROR TO SIMULATE !MODELSTATE.ISVALID ** //
            personsController.ModelState.AddModelError("PersonName", "Person Name can't be blank.");

            // Act
            IActionResult actionResult = await personsController.Create(personAdd);

            // Assert // Assert type into viewresult, assert type is PAR, assert equal value to above PAR
            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().Be(personAdd);
        }

        [Fact]
        public async Task Create_PostRequest_ValidModelState()
        {
            // Arrange // Initialize PAR due to parameter of controller action method
            PersonAddRequest personAdd = _fixture.Create<PersonAddRequest>();

            // Initialize PR to be returned by mock method of AddPerson()
            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            // Create list of countryresponse type
            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            // Create object of PersonController type (!)
            PersonsController personsController = new PersonsController(_personsService, _countriesService);

            // Create Mock method for GetAllCountries()
            _countryMock
                .Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            // Create Mock method for GetAllCountries()
            _personMock
                .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponse);

            // Act
            IActionResult actionResult = await personsController.Create(personAdd);

            // Assert // Assert type into RedirectToActionResult
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(actionResult);
            // Assert that the redirect result is Index
            redirectResult.ActionName.Should().Be("Index");
        }

        #endregion

        #region Edit Test Cases

        [Fact]
        public async Task Edit_Valid_GetRequest_ShouldReturnPersonToBeUpdated()
        {
            // Arrange // Create list of countryresponse type
            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            // Create mock PersonResponse
            PersonResponse? personResponse = _fixture
                .Build<PersonResponse?>()
                .With(p => p.Gender, GenderEnum.Male.ToString())
                .Create();

            // Create mock PUR
            PersonUpdateRequest personUpdate = personResponse.ToPersonUpdateRequest();

            // Create object of PersonController type (!)
            PersonsController personsController = new PersonsController(_personsService, _countriesService);

            // Create Mock method for GetAllCountries()
            _countryMock
                .Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            // Create Mock method for GetPersonByPersonId()
            _personMock
                .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(personResponse);

            // Act
            IActionResult action = await personsController.Edit(personResponse.PersonId);

            // Assert
            ViewResult result = Assert.IsType<ViewResult>(action);
            result.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
            result.ViewData.Model.Should().BeEquivalentTo(personUpdate);
        }

        // ADD FURTHER TESTS HERE

        #endregion

    }
}
