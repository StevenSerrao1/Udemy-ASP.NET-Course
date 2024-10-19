using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO (Data Transfer Object) for UPDATING Person details
    /// </summary>
    public class PersonUpdateRequest
    {

        [Required(ErrorMessage = "Person Name is required")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email field cannot be blank")]
        [EmailAddress(ErrorMessage = "Email must be a valid email")]
        public string? PersonEmail { get; set; }

        public Guid? CountryId { get; set; }
        public DateTime? DOB { get; set; }
        public string? PersonAddress { get; set; }
        public GenderEnum? Gender { get; set; }
        public Guid? PersonId { get; set; }
        public bool ReceivesNewsletters { get; set; }

        public Person ToPerson()
        {
            // Converts "PersonAddRequest" into an object of "Person" type
            return new Person()
            {
                PersonAddress = PersonAddress,
                PersonId = PersonId,
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
