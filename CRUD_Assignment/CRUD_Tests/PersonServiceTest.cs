using System;
using System.Collections.Generic;
using Services;
using ServiceContracts;
using Xunit;
using ServiceContracts.DTO;
using Xunit.Abstractions;
using Xunit.Sdk;
using Entities;
using Moq;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ServiceContracts.Enums;
using AutoFixture; // used to create dummy data for mock objects based on the model class type
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using RepositoryContracts;
using System.Linq.Expressions;

namespace CRUD_Tests
{
    public class PersonServiceTest
    {

        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly IPersonsRepository _personsRepository;
        private readonly Mock<IPersonsRepository> _personsRepoMock;
        private readonly ICountriesRepository _countriesRepository;
        private readonly Mock<ICountriesRepository> _countriesRepoMock;
        private readonly ITestOutputHelper _outputHelper;
        private readonly IFixture _fixture;

        public PersonServiceTest(ITestOutputHelper output)
        {
            // Create new fixture using autofixture
            _fixture = new Fixture();

            // Create mock repo
            _personsRepoMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepoMock.Object;

            _countriesRepoMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepoMock.Object;

            // Init empty countries list
            var countriesMockData = new List<Country>() { };
            var personsMockData = new List<Person>() { };

            // Create mock version of dbContext 
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
            );

            // Assign object of dbContextMock into an ApplicationDbContext object
            ApplicationDbContext dbContext = dbContextMock.Object;

            // Create Mock DbSet of countries type
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesMockData);

            // Create Mock DbSet of persons type
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsMockData);

            _countriesService = new CountriesService(_countriesRepository);
            _personService = new PersonService(_personsRepository, _countriesService);

            _outputHelper = output;
        }

        #region AddPerson() test cases

        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            // Arrange
            PersonAddRequest? personAddRequest = null;

            // Act
            Func<Task> action = async () => await _personService.AddPerson(personAddRequest!);

            // ASSERT // now using Fluent Assertions
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddPerson_NullName_ToBeArgumentException()
        {
            // Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            Person person = personAddRequest.ToPerson();

            // When calling mock AddPerson, return same person value
            _personsRepoMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            //Act
            Func<Task> action = async () => await _personService.AddPerson(personAddRequest);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_NullEmail()
        {
            // Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, null as string)
                .Create();

            // Act
            Func<Task> action = async () => await _personService.AddPerson(personAddRequest);

            // Assert // with fluent assertions
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_FullPersonDetails_ToSucceed()
        {
            // Arrange
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmail.com")
                .Create();

            Person person = personAddRequest.ToPerson();
            PersonResponse personExpected = person.ToPersonResponse();

            // If we supply person details, this should return the same person details
            _personsRepoMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            // Act // _personService is NOW a mocked version of the initial service
            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);
            personExpected.PersonId = personResponse.PersonId;

            // Assert
            if (personResponse != null)
            {
                //Assert.True(personResponse.PersonId != Guid.Empty);
                personResponse.PersonId.Should().NotBeEmpty();
                personResponse.Should().Be(personExpected);
            }
        }

        #endregion

        #region GetPersonByPersonId test cases

        [Fact]
        public async Task GetPersonByPersonId_NullId_ToBeNull()
        {
            // Arrange
            Guid personBeforeMethod = Guid.Empty;

            // Act
            PersonResponse? personAfterMethod = await _personService.GetPersonByPersonId(personBeforeMethod)!;

            // Assert
            //Assert.Equal(Guid.Empty, personAfterMethod!.PersonId);
            personAfterMethod!.PersonId.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPersonByPersonId_ValidId_ToSucceed()
        {
            // Arrange

            // Act
            Person personAddRequest = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "stevensemail@gmail.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            PersonResponse personResponse = personAddRequest.ToPersonResponse();

            // Assert.NotEmpty(personResponse.PersonId.ToString());
            // personResponse.PersonId.ToString().Should().NotBeEmpty(); //FLUENT ASSERTION

            // Mock Implementation of GetPersonByPersonId method
            _personsRepoMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(personAddRequest);

            // Best practice is to have only one method in test case
            PersonResponse? personSearchedById = await _personService.GetPersonByPersonId(personResponse.PersonId)!;

            // Assert
            // Assert.NotNull(personSearchedById);
            personSearchedById.Should().NotBeNull(); // FLUENT ASSERTION
            // Assert.Equal(personResponse, personSearchedById);
            personSearchedById.Should().Be(personResponse); // FLUENT ASSERTION
        }

        #endregion

        #region GetAllPersons() test cases

        [Fact]
        public async Task GetAllPersons_EmptyList_ToBeEmpty()
        {
            List<Person> emptyList = new List<Person>();
            // Create mock instance of GetAllPersons()
            _personsRepoMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(emptyList); 

            // No arrange, only act
            List<PersonResponse> personList = await _personService.GetAllPersons();

            // Assert
            //Assert.Empty(personList);
            personList.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPersons_ValidList_ToSucceed()
        {
            // Act
            List<Person> persons = new List<Person>() {

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "manynames@hotmail.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "zugzwang@hotmail.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "donuts@gmail.com")
                .With(p => p.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personAddList = persons.Select(temp => temp.ToPersonResponse()).ToList();

            // Use IOutputHelper to print expected values
            //_outputHelper.WriteLine("Expected:");
            //foreach (PersonResponse person in personResponseListBeforeCall)
            //{
            //    _outputHelper.WriteLine(person.ToString());
            //}

            // CREATE MOCK IMPLEMENTATION OF GETALLPERSONS()
            _personsRepoMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            List<PersonResponse> personResponseListAfterCall = await _personService.GetAllPersons();
            // Use IOutputHelper to print actual values
            //_outputHelper.WriteLine("Actual:");
            //foreach (PersonResponse persona in personResponseListAfterCall)
            //{
            //    _outputHelper.WriteLine(persona.ToString());
            //}

            // Assert
            personResponseListAfterCall.Should().BeEquivalentTo(personAddList);
        }

        #endregion

        #region GetFilteredPersons() test cases

        // If no search results match the given fields, the method must return ALL People
        [Fact]
        public async Task GetFilteredPersons_EmptyText_ToBeSuccessful()
        {
            // Act
            List<Person> persons = new List<Person>() {

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "manynames@hotmail.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "zugzwang@hotmail.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "donuts@gmail.com")
                .With(p => p.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personAddList = persons.Select(temp => temp.ToPersonResponse()).ToList();
            _personsRepoMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            // Use IOutputHelper to print expected values
            //_outputHelper.WriteLine("Expected:");
            //foreach (PersonResponse person in personResponseListBeforeCall)
            //{
            //    _outputHelper.WriteLine(person.ToString());
            //}

            _personsRepoMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

            // The empty search below searches for the personname that has no value
            List<PersonResponse> personsListFromSearch = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            // Use IOutputHelper to print actual values
            //_outputHelper.WriteLine("Actual:");
            //foreach (PersonResponse persona in personsListFromSearch)
            //{
            //    _outputHelper.WriteLine(persona.ToString());
            //}

            // Assert
            personsListFromSearch.Should().BeEquivalentTo(personAddList);
        }


        //If search results match the NAME field, the method must return specified person
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonEmail_ToBeSuccessful()
        {
            // Act
            List<Person> persons = new List<Person>() {

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "manynames@hotmail.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "zugzwang@hotmail.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "donuts@gmail.com")
                .With(p => p.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personAddList = persons.Select(temp => temp.ToPersonResponse()).ToList();
            _personsRepoMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            // Use IOutputHelper to print expected values
            //_outputHelper.WriteLine("Expected:");
            //foreach (PersonResponse person in personResponseListBeforeCall)
            //{
            //    _outputHelper.WriteLine(person.ToString());
            //}

            _personsRepoMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

            // The empty search below searches for the personname that has no value
            List<PersonResponse> personsListFromSearch = await _personService.GetFilteredPersons(nameof(Person.PersonEmail), "gz");

            // Use IOutputHelper to print actual values
            //_outputHelper.WriteLine("Actual:");
            //foreach (PersonResponse persona in personsListFromSearch)
            //{
            //    _outputHelper.WriteLine(persona.ToString());
            //}

            // Assert
            personsListFromSearch.Should().BeEquivalentTo(personAddList);
        }

        #endregion

        #region GetSortedPersons() test cases

        //If sorted based on personName in DESCENDING order, it should return List<PersonResponse> in DESCENDING order
        [Fact]
        public async Task GetSortedPersons_SearchByPersonName_ToSucceed()
        {
            // Act
            List<Person> persons = new List<Person>() {

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "manynames@hotmail.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "zugzwang@hotmail.com")
                .With(p => p.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(p => p.PersonEmail, "donuts@gmail.com")
                .With(p => p.Country, null as Country)
                .Create()
            };

            List<PersonResponse> personAddList = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _personsRepoMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            // Initialize empty list for storing EXPECTED people
            List<PersonResponse> allPersons = await _personService.GetAllPersons();

            // Use IOutputHelper to print expected values
            //_outputHelper.WriteLine("Expected List of Persons:");
            //foreach (PersonResponse person in personResponseListBeforeCall)
            //{
            //    _outputHelper.WriteLine(person.ToString());
            //}

            // Get list of people AFTER using Sort method in order to get ACTUAL value
            List<PersonResponse> personsListFromSort = await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), ServiceContracts.Enums.SortOrderEnum.Ascending);

            // Use IOutputHelper to print actual values
            //_outputHelper.WriteLine("Actual:");
            //foreach (PersonResponse persona in personsListFromSort)
            //{
            //    _outputHelper.WriteLine(persona.ToString());
            //}

            // Call situational sort method in order to get EXPECTED value
            //personResponseListBeforeCall = personResponseListBeforeCall.OrderBy(person => person.DOB).ToList();

            // Assert
            //for (int i = 0; i < personResponseListBeforeCall.Count; i++)
            //{
            //    // Assert.Equal(personResponseListBeforeCall[i], personsListFromSort[i]);
            //    personsListFromSort[i].Should().Be(personResponseListBeforeCall[i]); // FLUENT ASSERTION
            //}

            personsListFromSort.Should().BeInAscendingOrder(temp => temp.PersonName);
        }

        #endregion

        #region UpdatePerson() test cases

        // If PersonUpdateRequest is null, throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? pur = null;

            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personService.UpdatePerson(pur);
            //});

            Func<Task> action = async () => await _personService.UpdatePerson(pur);

            await action.Should().ThrowAsync<ArgumentNullException>(); // FLUENT ASSERTION
        }


        // If PersonId is invalid, throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidId_ToBeArgumentException()
        {
            PersonUpdateRequest? pur = _fixture
                .Build<PersonUpdateRequest>()
                .With(pur => pur.PersonId, Guid.NewGuid())
                .Create();

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personService.UpdatePerson(pur);
            //});

            Func<Task> action = async () => await _personService.UpdatePerson(pur);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // If PersonUpdateRequest is valid, return UPDATED PersonResponse object
        // In this case, PersonName and PersonEmail will be updated
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation()
        {
            // Create dummy person
           Person person = _fixture.Build<Person>()
                .With(p => p.PersonEmail, "abc@123.com")
                .With(p => p.Country, null as Country)
                .With(p => p.Gender, GenderEnum.Male.ToString())
                .Create();

            // Convert fromn 'Person' to 'PersonResponse' data type
            PersonResponse person_response_from_add = person.ToPersonResponse();

            // Convert to person update request in order to update values
            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();

            // Create mock method implementation
            _personsRepoMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);

            // Create another mock method implementation
            _personsRepoMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            PersonResponse person_response_from_update = await _personService.UpdatePerson(person_update_request);          

            //Assert
            person_response_from_update.Should().Be(person_response_from_add);

        }
        #endregion

        #region DeletePerson() test cases

        [Fact]
        // Check if Personid null, in which case the method returns false
        public async Task DeletePerson_NullId()
        {
            // Arrange
            // Create new PersonResponse type to test with EMPTY Id
            PersonResponse person = new PersonResponse() { PersonId = Guid.NewGuid() };

            // Act
            // Check if the personId is found, which it shouldn't be
            bool personFound = await _personService.DeletePersonByPersonId(person.PersonId);

            // Assert
            // Assert that the result returned is false
            // Assert.False(personFound);
            personFound.Should().BeFalse();
        }

        [Fact]
        public async Task DeletePerson_ValidPersonToDelete()
        {
            // Arrange
            Person person = _fixture.Build<Person>()
                .With(p => p.PersonEmail, "manynames@hotmail.com")
                .With(p => p.Country, null as Country)
                .Create();

            PersonResponse addy = person.ToPersonResponse();

            _personsRepoMock
                .Setup(temp => temp.DeletePersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _personsRepoMock
                .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            // Use output helper to confirm conversion of PersonAddRequest items to PersonResponse
            //_outputHelper.WriteLine("Expected:");
            //foreach (PersonResponse person in people)
            //{
            //    _outputHelper.WriteLine(person.ToString());
            //}

            // Act
            // If person is not null, check to see if Id is valid
            // Clone list before deleting
            // List<PersonResponse> peopleAfterDeletion = people;

            // Select person from list and do null check
            PersonResponse personBeingChecked = addy;
            if (personBeingChecked == null) throw new ArgumentNullException();

            // Delete person using custom method
            bool isDeleted = await _personService.DeletePersonByPersonId(personBeingChecked.PersonId);

            // Assert
            // Assert that the lists match with the given person deleted
            isDeleted.Should().Be(true);
        }

        #endregion
    }
}
