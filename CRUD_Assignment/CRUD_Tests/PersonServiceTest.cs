﻿using System;
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

namespace CRUD_Tests
{
    public class PersonServiceTest
    {

        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _outputHelper;

        public PersonServiceTest(ITestOutputHelper output)
        {
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

            _countriesService = new CountriesService(dbContext);
            _personService = new PersonService(dbContext, _countriesService);

            _outputHelper = output;
        }

        #region AddPerson() test cases

        [Fact]
        public async Task AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequest? personAddRequest = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                // Act
               async () => await _personService.AddPerson(personAddRequest!)
            );
        }

        [Fact]
        public async Task AddPerson_NullName()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };            

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(
                // Act
                async () => await _personService.AddPerson(personAddRequest)
            );
        }

        [Fact]
        public async Task AddPerson_NullEmail()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonEmail = null };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(
                // Act
                async () => await _personService.AddPerson(personAddRequest)
            );
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            // Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Steve",
                PersonEmail = "stevesemail@gmail.com",
                CountryId = Guid.NewGuid(),
                DOB = new DateTime(2000, 02, 23),
                PersonAddress = "2101 Hillcrest Ave.",
                Gender = 0,
                ReceivesNewsletters = true
            };

            // Act
            PersonResponse? personResponse = await _personService.AddPerson(personAddRequest);
            List<PersonResponse>? listOfPersonResponses = await _personService.GetAllPersons();

            // Assert
            if (personResponse != null && listOfPersonResponses != null)
            {
                Assert.True(personResponse.PersonId != Guid.Empty);
                Assert.Contains(personResponse, listOfPersonResponses);
            }
        }

        #endregion

        #region GetPersonByPersonId test cases

        [Fact]
        public async Task GetPersonByPersonId_NullId()
        {
            // Arrange
            Guid personBeforeMethod = Guid.Empty;

            // Act
            PersonResponse? personAfterMethod = await _personService.GetPersonByPersonId(personBeforeMethod)!;

            // Assert
            Assert.Equal(Guid.Empty, personAfterMethod!.PersonId);
        }

        [Fact]
        public async Task GetPersonByPersonId_ValidId()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "South-Africa"
            };
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

            Assert.NotNull(countryResponse);
            Assert.NotEqual(Guid.Empty, countryResponse.CountryID);

            // Act
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Steve",
                PersonEmail = "stevesemail@gmail.com",
                CountryId = countryResponse.CountryID,
                DOB = new DateTime(2000, 02, 23),
                PersonAddress = "30 Rockefeller Ave",
                Gender = 0,
                ReceivesNewsletters = true
            };

            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

            Assert.NotEmpty(personResponse.PersonId.ToString());

            PersonResponse? personSearchedById = await _personService.GetPersonByPersonId(personResponse.PersonId)!;

            // Assert
            Assert.NotNull(personSearchedById);
            Assert.Equal(personResponse, personSearchedById);
        }

        #endregion

        #region GetAllPersons() test cases

        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            // No arrange, only act
            List<PersonResponse> personList = await _personService.GetAllPersons();

            // Assert
            Assert.Empty(personList);
        }

        [Fact]
        public async Task GetAllPersons_ValidList()
        {
            // Arrange
            // Add multiple countries
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "South-Africa"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);

            // Act
            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Steve",
                PersonEmail = "stevesemail@gmail.com",
                CountryId = countryResponse1.CountryID,
                DOB = new DateTime(2000, 02, 23),
                PersonAddress = "30 Rockefeller Ave",
                Gender = 0,
                ReceivesNewsletters = true
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Spencer",
                PersonEmail = "zugzwang@gmail.com",
                CountryId = countryResponse2.CountryID,
                DOB = new DateTime(1980, 03, 09),
                PersonAddress = "Dolbow, Quantico, VA 22134",
                Gender = 0,
                ReceivesNewsletters = false
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Homer",
                PersonEmail = "donuts@gmail.com",
                CountryId = countryResponse2.CountryID,
                DOB = new DateTime(1960, 01, 01),
                PersonAddress = "91 Evergreen Terrace, Springfield",
                Gender = 0,
                ReceivesNewsletters = true
            };

            List<PersonAddRequest> personAddList = new List<PersonAddRequest>()
            { personAddRequest1, personAddRequest2, personAddRequest3 };

            List<PersonResponse> personResponseListBeforeCall = new List<PersonResponse>();

            foreach(PersonAddRequest addRequest in personAddList)
            {
                personResponseListBeforeCall.Add(await _personService.AddPerson(addRequest));
            }
            // Use IOutputHelper to print expected values
            _outputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponseListBeforeCall)
            {
                _outputHelper.WriteLine(person.ToString());
            }

            List<PersonResponse> personResponseListAfterCall = await _personService.GetAllPersons();
            // Use IOutputHelper to print actual values
            _outputHelper.WriteLine("Actual:");
            foreach (PersonResponse persona in personResponseListAfterCall)
            {
                _outputHelper.WriteLine(persona.ToString());
            }

            // Assert
            foreach (PersonResponse person in personResponseListBeforeCall)
            {
                Assert.Contains(person, personResponseListAfterCall);
            }
        }

        #endregion

        #region GetFilteredPersons() test cases

        // If no search results match the given fields, the method must return ALL People
        [Fact]
        public async Task GetFilteredPersons_EmptyText()
        {
            // Arrange
            // Add multiple countries
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "South-Africa"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);

            // Act
            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Steve",
                PersonEmail = "stevesemail@gmail.com",
                CountryId = countryResponse1.CountryID,
                DOB = new DateTime(2000, 02, 23),
                PersonAddress = "30 Rockefeller Ave",
                Gender = 0,
                ReceivesNewsletters = true
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Spencer",
                PersonEmail = "zugzwang@gmail.com",
                CountryId = countryResponse2.CountryID,
                DOB = new DateTime(1980, 03, 09),
                PersonAddress = "Dolbow, Quantico, VA 22134",
                Gender = 0,
                ReceivesNewsletters = false
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Homer",
                PersonEmail = "donuts@gmail.com",
                CountryId = countryResponse2.CountryID,
                DOB = new DateTime(1960, 01, 01),
                PersonAddress = "91 Evergreen Terrace, Springfield",
                Gender = 0,
                ReceivesNewsletters = true
            };

            List<PersonAddRequest> personAddList = new List<PersonAddRequest>()
            { personAddRequest1, personAddRequest2, personAddRequest3 };

            List<PersonResponse> personResponseListBeforeCall = new List<PersonResponse>();

            foreach (PersonAddRequest addRequest in personAddList)
            {
                personResponseListBeforeCall.Add(await _personService.AddPerson(addRequest));
            }

            // Use IOutputHelper to print expected values
            _outputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponseListBeforeCall)
            {
                _outputHelper.WriteLine(person.ToString());
            }

            // The empty search below searches for the personname that has no value
            List<PersonResponse> personsListFromSearch = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            // Use IOutputHelper to print actual values
            _outputHelper.WriteLine("Actual:");
            foreach (PersonResponse persona in personsListFromSearch)
            {
                _outputHelper.WriteLine(persona.ToString());
            }

            // Assert
            foreach (PersonResponse person in personResponseListBeforeCall)
            {
                Assert.Contains(person, personsListFromSearch);
            }
        }


        // If search results match the NAME field, the method must return specified person
        //[Fact]
        //public void GetFilteredPersons_SearchByPersonName()
        //{
        //    // Arrange
        //    // Add multiple countries and people
        //    List<PersonAddRequest> personAddList = _personService.AddCountriesAndPeople();

        //    List<PersonResponse> personResponseListBeforeCall = new List<PersonResponse>();

        //    foreach (PersonAddRequest addRequest in personAddList)
        //    {
        //        personResponseListBeforeCall.Add(_personService.AddPerson(addRequest));
        //    }
        //    // Use IOutputHelper to print expected values
        //    _outputHelper.WriteLine("Expected:");
        //    foreach (PersonResponse person in personResponseListBeforeCall)
        //    {
        //        _outputHelper.WriteLine(person.ToString());
        //    }

        //    List<PersonResponse> personsListFromSearch = _personService.GetFilteredPersons(nameof(Person.PersonEmail), "gz");

        //    // Use IOutputHelper to print actual values
        //    _outputHelper.WriteLine("Actual:");
        //    foreach (PersonResponse persona in personsListFromSearch)
        //    {
        //        _outputHelper.WriteLine(persona.ToString());
        //    }

        //    // Assert
        //    foreach (PersonResponse person in personResponseListBeforeCall)
        //    {   if(person.PersonEmail != null && person.PersonEmail.Contains("gz",  StringComparison.OrdinalIgnoreCase))
        //        {
        //           Assert.Contains(person, personsListFromSearch);
        //        }                                 
        //    }
        //}

        #endregion

        #region GetSortedPersons() test cases

        // If sorted based on personName in DESCENDING order, it should return List<PersonResponse> in DESCENDING order
        //[Fact]
        //public void GetSortedPersons_SearchByPersonName()
        //{
        //    // Arrange
        //    // Add multiple countries and people
        //    List<PersonAddRequest> personAddList = _personService.AddCountriesAndPeople();

        //    // Initialize empty list for storing EXPECTED people
        //    List<PersonResponse> personResponseListBeforeCall = new List<PersonResponse>();

        //    // Use regular add method to convert each PersonAddRequest -> Person -> PersonResponse
        //    foreach (PersonAddRequest addRequest in personAddList)
        //    {
        //        personResponseListBeforeCall.Add(_personService.AddPerson(addRequest));
        //    }

        //    // Use IOutputHelper to print expected values
        //    _outputHelper.WriteLine("Expected List of Persons:");
        //    foreach (PersonResponse person in personResponseListBeforeCall)
        //    {
        //        _outputHelper.WriteLine(person.ToString());
        //    }

        //    // Get list of people AFTER using Sort method in order to get ACTUAL value
        //    List<PersonResponse> personsListFromSort = _personService.GetSortedPersons(personResponseListBeforeCall, nameof(Person.DOB), ServiceContracts.Enums.SortOrderEnum.Ascending);

        //    // Use IOutputHelper to print actual values
        //    _outputHelper.WriteLine("Actual:");
        //    foreach (PersonResponse persona in personsListFromSort)
        //    {
        //        _outputHelper.WriteLine(persona.ToString());
        //    }

        //    // Call situational sort method in order to get EXPECTED value
        //    personResponseListBeforeCall = personResponseListBeforeCall.OrderBy(person => person.DOB).ToList();

        //    // Assert
        //    for(int i= 0; i < personResponseListBeforeCall.Count; i++)
        //    {
        //        Assert.Equal(personResponseListBeforeCall[i], personsListFromSort[i]);
        //    }        
        //}

        #endregion

        #region UpdatePerson() test cases

        // If PersonUpdateRequest is null, throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? pur = null;         

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.UpdatePerson(pur);
            });
        }

        // If PersonId is invalid, throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidId()
        {
            PersonUpdateRequest? pur = new PersonUpdateRequest() { PersonId = Guid.NewGuid() };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personService.UpdatePerson(pur);
            });
        }

        // If PersonUpdateRequest is valid, return UPDATED PersonResponse object
        // In this case, PersonName and PersonEmail will be updated
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryId = country_response_from_add.CountryID, PersonAddress = "Abc road", DOB = DateTime.Parse("2000-01-01"), PersonEmail = "abc@example.com", Gender = GenderEnum.Male, ReceivesNewsletters = true };

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.PersonEmail = "william@example.com";

            //Act
            PersonResponse person_response_from_update = await _personService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonId(person_response_from_update.PersonId)!;

            //Assert
            Assert.Equal(person_response_from_get, person_response_from_update);

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
            bool personFound = await _personService.DeletePerson(person.PersonId);

            // Assert
            // Assert that the result returned is false
            Assert.False(personFound);
        }

        //[Fact]
        //public void DeletePerson_ValidPersonToDelete()
        //{
        //    // Arrange
        //    // Create new PersonResponse type to test with EMPTY Id
        //    List<PersonResponse> people = new List<PersonResponse>();
        //    List<PersonAddRequest> addies = _personService.AddCountriesAndPeople();
        //    foreach(PersonAddRequest add in addies)
        //    {
        //        people.Add(_personService.AddPerson(add));
        //    }

        //    // Use output helper to confirm conversion of PersonAddRequest items to PersonResponse
        //    _outputHelper.WriteLine("Expected:");
        //    foreach (PersonResponse person in people)
        //    {
        //        _outputHelper.WriteLine(person.ToString());
        //    }

        //    // Act
        //    // If person is not null, check to see if Id is valid
        //    // Clone list before deleting
        //    List<PersonResponse> peopleAfterDeletion = people;

        //    // Select person from list and do null check
        //    PersonResponse personBeingChecked = people[1];
        //    if (personBeingChecked == null) throw new ArgumentNullException();

        //    // Delete person using custom method
        //    _personService.DeletePerson(personBeingChecked.PersonId);

        //    // Remove person from list in order to compare lists
        //    peopleAfterDeletion.Remove(personBeingChecked);

        //    // Use output helper to confirm conversion of PersonAddRequest items to PersonResponse
        //    _outputHelper.WriteLine("Actual:");
        //    foreach (PersonResponse person in people)
        //    {
        //        _outputHelper.WriteLine(person.ToString());
        //    }

        //    // Assert
        //    // Assert that the lists match with the given person deleted
        //    Assert.Equal(peopleAfterDeletion, people);
        //}


        #endregion
    }
}
