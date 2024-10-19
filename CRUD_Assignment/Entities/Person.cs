using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        public string? PersonName { get; set; }
        public string? PersonEmail { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? CountryId { get; set; }
        public DateTime? DOB { get; set; }
        public string? PersonAddress { get; set; }
        public string? Gender { get; set; }
        public bool ReceivesNewsletters { get; set; }
    }
}
