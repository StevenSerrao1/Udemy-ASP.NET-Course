using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO (Data Transfer Object) for inserting a new Person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person Name is required")]
        public string? PersonName { get; set; }


        [Required(ErrorMessage = "Email field cannot be blank")]
        [EmailAddress(ErrorMessage = "Email must be a valid email")]
        [DataType(DataType.EmailAddress)]
        public string? PersonEmail { get; set; }


        [Required(ErrorMessage = "Select a country")]
        public Guid? CountryId { get; set; }


        [Required(ErrorMessage = "Date of Birth cannot be empty")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }


        [Required(ErrorMessage = "Enter valid address")]
        public string? PersonAddress { get; set; }


        [Required(ErrorMessage = "Select a gender")]
        public GenderEnum? Gender { get; set; }


        public bool ReceivesNewsletters { get; set; }


        public Person ToPerson()
        {
            // Converts "PersonAddRequest" into an object of "Person" type
            return new Person()
            {
                PersonAddress = PersonAddress,
                PersonName = PersonName,
                PersonEmail = PersonEmail,
                DOB = DOB,
                CountryId = CountryId,
                Gender = Gender.ToString(),
                ReceivesNewsletters = (bool)ReceivesNewsletters
            };
        }
    }
}
