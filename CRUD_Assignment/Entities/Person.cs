using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }

        [StringLength(40)]
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? PersonEmail { get; set; }

        public Guid? CountryId { get; set; }

        public DateTime? DOB { get; set; }

        [StringLength(100)]
        public string? PersonAddress { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        public bool ReceivesNewsletters { get; set; }

        public string? TIN { get; set; }
    }
}
