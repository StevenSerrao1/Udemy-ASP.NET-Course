using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceContracts.Enums;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonService
    {
        /// <summary>
        /// Adds a person into a list of person type List<Person>
        /// </summary>
        /// <param name="personAddRequest"></param>
        /// <returns></returns>
        public Task<PersonResponse> AddPerson(PersonAddRequest personAddRequest);

        /// <summary>
        /// Returns a list of all person objects in the mock database
        /// </summary>
        /// <returns></returns>
        public Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns an object of PersonResponse type based on Guid PersonId
        /// </summary>
        /// <param name="personID"></param>
        /// <returns>PersonResponse</returns>
        public Task<PersonResponse?>? GetPersonByPersonId(Guid? personID);

        /// <summary>
        /// Get a list of people matching certain criteria
        /// </summary>
        /// <param name="searchBy">Used to dictate the field being searched</param>
        /// <param name="searchString">Used to dictate the value being searched within that field</param>
        /// <returns>List of PersonResponse type with all matching people</returns>
        public Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Returns a List<PersonResponse> that contains sorted people based on 3 parameters
        /// </summary>
        /// <param name="allPersons">Accepts a list of PersonResponse type that must be sorted</param>
        /// <param name="sortBy">Accepts the field by which the list must be sorted</param>
        /// <param name="sortOrder">Accepts the order (ascending or descending) by which the list must be sorted</param>
        /// <returns>List<PersonResponse></returns>
        public Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderEnum sortOrder);

        /// <summary>
        /// Returns an UPDATED VERSION of PersonResponse object
        /// </summary>
        /// <param name="personUpdateRequest"></param>
        /// <returns>PersonResponse</returns>
        public Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes a Person from the Database store via the PersonId parameter
        /// </summary>
        /// <param name="PersonId"></param>
        /// <returns>Boolean value determining success in deleting person</returns>
        public Task<bool> DeletePerson(Guid? PersonId);

    }
}
