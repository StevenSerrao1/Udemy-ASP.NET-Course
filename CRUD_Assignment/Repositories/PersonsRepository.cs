using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PersonsRepository> _logger;
        public PersonsRepository(ApplicationDbContext db, ILogger<PersonsRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Person> AddPerson(Person person)
        {
            _db.Add(person);
            await _db.SaveChangesAsync();

            return person;
        }

        public async Task<bool> DeletePersonByPersonId(Guid personId)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(temp => temp.PersonId == personId));
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            // Logging message of Info type
            _logger.LogInformation("GetAllPersons() method of PersonRepository");

            return await _db.Persons
                .Include("Country")
                .ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons
                .Include("Country")
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonId(Guid personId)
        {
            return await _db.Persons
                .Include("Country")
                .FirstOrDefaultAsync(temp => temp.PersonId == personId);             
        }

        public async Task<Person?> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
            if (matchingPerson == null) return person;

            matchingPerson.PersonEmail = person.PersonEmail;
            matchingPerson.PersonName = person.PersonName;
            matchingPerson.DOB = person.DOB;
            matchingPerson.Gender = person.Gender;
            matchingPerson.PersonAddress = person.PersonAddress;
            matchingPerson.ReceivesNewsletters = person.ReceivesNewsletters;
            matchingPerson.CountryId = person.CountryId;

            return matchingPerson;
        }
    }
}
