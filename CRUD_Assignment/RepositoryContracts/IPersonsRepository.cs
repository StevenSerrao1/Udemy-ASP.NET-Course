using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents Data Access Logic for managing Person entity
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds a Person object to data store
        /// </summary>
        /// <param name="person">Person to add</param>
        /// <returns>Object of Person type after being added to data store</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Retrieve a List<Person> of Person objects in data store
        /// </summary>
        /// <returns>List of Person type</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Retreieve a person object based on personId
        /// </summary>
        /// <param name="personId">PersonId to search</param>
        /// <returns>Object of Person type or null</returns>
        Task<Person?> GetPersonByPersonId(Guid personId);

        /// <summary>
        /// Returns all person objects based on a specified conditionS
        /// </summary>
        /// <param name="predicate">LINQ expression to filter Person objects</param>
        /// <returns>All matching persons with given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes a person object based on the personId
        /// </summary>
        /// <param name="personId">personId of person to be deleted</param>
        /// <returns>Boolean value indicating success of person deletion</returns>
        Task<bool> DeletePersonByPersonId(Guid personId);

        /// <summary>
        /// Updates person details
        /// </summary>
        /// <param name="person">Person object to be updated</param>
        /// <returns>Object of Person type with updated details</returns>
        Task<Person?> UpdatePerson(Person person);

    }
}
