using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using Services.Helpers;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;
using Entities;
using Services;
using ServiceContracts.Enums;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly PersonsDbContext _personsDb;
        private readonly ICountriesService _countriesService;

        public PersonService(PersonsDbContext dbContext, ICountriesService countriesService)
        {
            _personsDb = dbContext;
            _countriesService = countriesService;

        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest personAddRequest)
        {
            // Check if personAddRequest is not null
            if (personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));

            // Validate model contexts
            ValidationHelpers.ValidateObject(personAddRequest);

            // Convert "personAddRequest" from PersonAddRequest type to Person type
            Person person = personAddRequest.ToPerson();

            // Generate a new PersonId
            person.PersonId = Guid.NewGuid();

            // Add into List<Person>
            _personsDb.Persons.Add(person);
            //_personsDb.sp_AddPerson(person);
            // Save changes to DbSet - UNNECESSARY WHEN USING STORED PROCEDURES
            await _personsDb.SaveChangesAsync();

            // Convert "person" from Person type to PersonResponse type WITH CountryId
            PersonResponse personResponse = person.ToPersonResponse();

            // Return PersonResponse object WITH generated PersonId
            return personResponse;

        }

        public List<Person> AddMockPeople()
        {
            List<Person> preAdders = new List<Person>()
            {
                new Person()
                {
                    PersonName = "Steve",
                    PersonEmail = "stevesemail@gmail.com",
                    CountryId = Guid.Parse("EE878C05-7FE6-4F23-8C44-A4CD10D410A2"),
                    PersonId = Guid.Parse("123B6BED-36D2-495C-96DB-4B4325219A42"),
                    DOB = new DateTime(2000, 02, 23),
                    PersonAddress = "30 Rockefeller Ave, Klerskdorp, North-West",
                    Gender = "Male",
                    ReceivesNewsletters = true
                },
                new Person()
                {
                    PersonName = "Spencer",
                    PersonEmail = "zugzwang@gmail.com",
                    CountryId = Guid.Parse("02CA2B03-63BD-4891-9A59-E92436CA0F33"),
                    PersonId = Guid.Parse("E0422365-6F36-4798-AD50-4F3E6258D878"),
                    DOB = new DateTime(1980, 03, 09),
                    PersonAddress = "Dolbow, Quantico, Virginia",
                    Gender = "Male",
                    ReceivesNewsletters = false
                },
                new Person()
                {
                    PersonName = "Homer",
                    PersonEmail = "donuts@gmail.com",
                    CountryId = Guid.Parse("02CA2B03-63BD-4891-9A59-E92436CA0F33"),
                    PersonId = Guid.Parse("BE34573F-020E-4615-ABE2-EFC26A17377C"),
                    DOB = new DateTime(1960, 01, 01),
                    PersonAddress = "91 Evergreen Terrace, Springfield, Massachusetts",
                    Gender = "Male",
                    ReceivesNewsletters = true
                },
                new Person()
                {
                    PersonName = "Fiona",
                    PersonEmail = "fuckoff@gmail.com",
                    CountryId = Guid.Parse("02CA2B03-63BD-4891-9A59-E92436CA0F33"),
                    PersonId = Guid.Parse("8BD19D48-A0E7-4F03-BCB6-054C968D39DB"),
                    DOB = new DateTime(1991, 04, 04),
                    PersonAddress = "14 Rathole Ave., Chicago, Illinois",
                    Gender = "Female",
                    ReceivesNewsletters = false
                },
                new Person()
                {
                    PersonName = "Ryk",
                    PersonEmail = "backstroke@gmail.com",
                    CountryId = Guid.Parse("EE878C05-7FE6-4F23-8C44-A4CD10D410A2"),
                    PersonId = Guid.Parse("0953EF08-4F88-4E69-869F-E9A15F68E846"),
                    DOB = new DateTime(1941, 11, 30),
                    PersonAddress = "940 Groothuis, Cape Town, Western Cape",
                    Gender = "Male",
                    ReceivesNewsletters = true
                },
                new Person()
                {
                    PersonName = "Max",
                    PersonEmail = "needforspeed@gmail.com",
                    CountryId = Guid.Parse("641670A5-FED6-44E9-AB25-E3B18C6DC7C8"),
                    PersonId = Guid.Parse("62D41E59-C811-4BBF-B1BB-557F9E0D3B6C"),
                    DOB = new DateTime(1997, 10, 15),
                    PersonAddress = "Verstappen House, Berlin, Germany",
                    Gender = "Male",
                    ReceivesNewsletters = false
                }
            };

            return preAdders;
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await _personsDb.Persons.Include("Country").ToListAsync();
            // Doing it WITH a stored db procedure
            return persons.Select(person => person.ToPersonResponse()).ToList();

            // One way to do it without stored procedure
            // SELECT * from Persons
            //return _personsDb.Persons.ToList().Select(person => ConvertPersonToPersonResponse(person)).ToList();

            // Below, it is impossible to use a custom method within a LINQ to entity expression
            //return _personsDb.Persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
        }

        public async Task<PersonResponse?>? GetPersonByPersonId(Guid? personID)
        {
            // Check if PersonId is not null
            if (personID == null) return new PersonResponse() { PersonName = "ID is null"};

            // Get Matching person from List<Person> by PersonId
            Person? person = await _personsDb.Persons.Include("Country").FirstOrDefaultAsync(persona => persona.PersonId == personID);

            // Convert matching person from Person to PersonResponse object type
            if (person == null) return new PersonResponse() { PersonName = "PERSON is null" };

            // Return PersonResponse object
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            // Get the default list of all people
            List<PersonResponse> allPeople = await GetAllPersons();

            // Create List<PersonResponse> to store matching people
            List<PersonResponse> matchingPeople = allPeople;

            // Check if "searchBy" is not null and return all people if so
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPeople;
            }

            // Get matching people from List<Person> based on parameters
            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPeople = allPeople.Where(person =>
                    (!string.IsNullOrEmpty(person.PersonName) ?
                    person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true
                )).ToList(); break;

                case nameof(Person.PersonEmail):
                    matchingPeople = allPeople.Where(person =>
                    (!string.IsNullOrEmpty(person.PersonEmail) ?
                    person.PersonEmail.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true
                )).ToList(); break;

                case nameof(Person.PersonAddress):
                    matchingPeople = allPeople.Where(person =>
                    (!string.IsNullOrEmpty(person.PersonAddress) ?
                    person.PersonAddress.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true
                )).ToList(); break;

                case nameof(Person.ReceivesNewsletters):
                    matchingPeople = allPeople.Where(person =>
                    person.ReceivesNewsletters.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)
                ).ToList(); break;

                case nameof(Person.DOB):
                    matchingPeople = allPeople.Where(person =>
                    (person.DOB != null) ?
                    person.DOB.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true
                ).ToList(); break;

                case nameof(Person.Gender):
                    matchingPeople = allPeople.Where(person =>
                    (!string.IsNullOrEmpty(person.Gender) ?
                    person.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase) : true
                )).ToList(); break;

                default: matchingPeople = allPeople; break;
            }

            // Return all matching PersonResponse objects
            return matchingPeople;
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderEnum sortOrder)
        {
            // If sortBy param is empty/null, return allPersons as is
            if (string.IsNullOrEmpty(sortBy)) return allPersons;

            // Create a switch EXPRESSION statement to determine the sortedPersons list
            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                // sortBy = PersonName / sortOrder = Ascending
                (nameof(PersonResponse.PersonName), SortOrderEnum.Ascending) => allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                // sortBy = PersonName / sortOrder = Descending
                (nameof(PersonResponse.PersonName), SortOrderEnum.Descending) => allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                // sortBy = PersonEmail / sortOrder = Ascending
                (nameof(PersonResponse.PersonEmail), SortOrderEnum.Ascending) => allPersons.OrderBy(p => p.PersonEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                // sortBy = PersonEmail / sortOrder = Descending
                (nameof(PersonResponse.PersonEmail), SortOrderEnum.Descending) => allPersons.OrderByDescending(p => p.PersonEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                // sortBy = PersonDOB / sortOrder = Ascending
                (nameof(PersonResponse.DOB), SortOrderEnum.Ascending) => allPersons.OrderBy(p => p.DOB).ToList(),

                // sortBy = PersonDOB / sortOrder = Descending
                (nameof(PersonResponse.DOB), SortOrderEnum.Descending) => allPersons.OrderByDescending(p => p.DOB).ToList(),

                // sortBy = PersonAge / sortOrder = Ascending
                (nameof(PersonResponse.Age), SortOrderEnum.Ascending) => allPersons.OrderBy(p => p.Age).ToList(),

                // sortBy = PersonAge / sortOrder = Descending
                (nameof(PersonResponse.Age), SortOrderEnum.Descending) => allPersons.OrderByDescending(p => p.Age).ToList(),

                // sortBy = PersonGender / sortOrder = Ascending
                (nameof(PersonResponse.Gender), SortOrderEnum.Ascending) => allPersons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                // sortBy = PersonGender / sortOrder = Descending
                (nameof(PersonResponse.Gender), SortOrderEnum.Descending) => allPersons.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                // sortBy = PersonCountry / sortOrder = Ascending
                (nameof(PersonResponse.Country), SortOrderEnum.Ascending) => allPersons.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                // sortBy = PersonCountry / sortOrder = Descending
                (nameof(PersonResponse.Country), SortOrderEnum.Descending) => allPersons.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                // sortBy = PersonReceivesNewsletters / sortOrder = Ascending
                (nameof(PersonResponse.ReceivesNewsletters), SortOrderEnum.Ascending) => allPersons.OrderBy(p => p.ReceivesNewsletters).ToList(),

                // sortBy = PersonReceivesNewsletters / sortOrder = Descending
                (nameof(PersonResponse.ReceivesNewsletters), SortOrderEnum.Descending) => allPersons.OrderByDescending(p => p.ReceivesNewsletters).ToList(),

                // sortBy = PersonAddress / sortOrder = Ascending
                (nameof(PersonResponse.PersonAddress), SortOrderEnum.Ascending) => allPersons.OrderBy(p => p.PersonAddress, StringComparer.OrdinalIgnoreCase).ToList(),

                // sortBy = PersonAddress / sortOrder = Descending
                (nameof(PersonResponse.PersonAddress), SortOrderEnum.Descending) => allPersons.OrderByDescending(p => p.PersonAddress, StringComparer.OrdinalIgnoreCase).ToList(),

                _ => allPersons
            };

            return await Task.FromResult(sortedPersons);
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            // Check if PUR is not null
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(Person));

            // Validate all properties of PUR
            ValidationHelpers.ValidateObject(personUpdateRequest);

            // Get matching Person from List<Person> based on PersonId
            Person? matchingPerson = await _personsDb.Persons.FirstOrDefaultAsync(p => p.PersonId == personUpdateRequest.PersonId);

            // Check if matchingPerson is not null
            if (matchingPerson == null) throw new ArgumentException("PersonId does not exist");

            // Call the stored procedure through the DbContext
            _personsDb.sp_UpdatePerson(
                personUpdateRequest.PersonId,
                personUpdateRequest.DOB,
                personUpdateRequest.Gender.ToString(),
                personUpdateRequest.PersonAddress,
                personUpdateRequest.PersonEmail,
                personUpdateRequest.PersonName,
                personUpdateRequest.ReceivesNewsletters,
                personUpdateRequest.CountryId
            );

            // Save changes to entity object
            // _personsDb.SaveChanges(); // UPDATE

            return matchingPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? PersonId)
        {
            // Check if ID is null
            if (PersonId == null) throw new ArgumentNullException("PersonId is null");

            // Get matching Person from List<Person>
            Person? matchingPerson = await _personsDb.Persons.FirstOrDefaultAsync(p => p.PersonId == PersonId);

            // Check if Person is null
            if (matchingPerson == null) return false;

            // Delete Person object from List
            _personsDb.sp_DeletePerson(matchingPerson);

            // Save changes to DbSet by saving DELETION of person - UNNECESSARY WHEN USING STORED PROCEDURES
            //_personsDb.SaveChanges(); // DELETE

            // Return boolean value to indicate success of deletion
            return _personsDb.Persons.Contains(matchingPerson) ? false : true;
        }
    }
}
