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

    }
}
