using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;
using Entities;
using System.Xml.Linq;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO Class used to return type of PersonResponse for methods used on Person type
    /// </summary>
    public class PersonResponse
    { 
        public string? PersonName { get; set; }
        public string? PersonEmail { get; set; }      
        public string? Country { get; set; }      
        public Guid? CountryId { get; set; }
        public DateTime? DOB { get; set; }
        public string? PersonAddress { get; set; }
        public string? Gender { get; set; }
        public bool ReceivesNewsletters { get; set; }
        public Guid PersonId { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the parameter object data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse personResponse = (PersonResponse)obj;

            return
                PersonId == personResponse.PersonId &&
                PersonEmail == personResponse.PersonEmail &&
                ReceivesNewsletters == personResponse.ReceivesNewsletters &&
                DOB == personResponse.DOB &&
                PersonName == personResponse.PersonName &&
                Gender == personResponse.Gender &&
                CountryId == personResponse.CountryId &&
                PersonAddress == personResponse.PersonAddress;
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person Id: {PersonId}\nPerson Name: {PersonName}\nPerson Email: {PersonEmail}\nDOB: {DOB}\nGender: {Gender}\nAddress: {PersonAddress}\nCountry ID: {CountryId}\n Country: {Country}\nReceives Newsletters? {ReceivesNewsletters}\n\n";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonAddress = PersonAddress,
                PersonId = PersonId,
                PersonName = PersonName,
                PersonEmail = PersonEmail,
                DOB = DOB,
                CountryId = CountryId,
                Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), Gender, true),
                ReceivesNewsletters = ReceivesNewsletters
            };
        }
    }

    public static class PersonExtensions
    {

        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonAddress = person.PersonAddress,
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                PersonEmail = person.PersonEmail,
                DOB = person.DOB,
                CountryId = person.CountryId,
                Gender = person.Gender,
                ReceivesNewsletters = person.ReceivesNewsletters,
                Age = ((person.DOB != null) ? Math.Floor((DateTime.Now - person.DOB.Value).TotalDays / 365.25) : null),
                Country = person.Country?.CountryName
            };
        }

    }
}
