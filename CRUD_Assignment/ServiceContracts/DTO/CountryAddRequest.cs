using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServiceContracts.DTO
{
    public class CountryAddRequest
    {
        /// <summary>
        /// DTO Class for adding a new country
        /// </summary>

       public string? CountryName { get; set; }

        // This method turns a request with just a country name, into a country object
        public Country ToCountry()
        {
            return new Country { CountryName = this.CountryName };
        }

    }
}
