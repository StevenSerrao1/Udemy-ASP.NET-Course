using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelpers
    {

        internal static void ValidateObject(object obj)
        {
            // Create validationcontext object that references model OBJECT
            ValidationContext validationContext = new ValidationContext(obj);
            // Create list of validationresults to store validation errors
            List<ValidationResult> validationResults = new List<ValidationResult>();
            // Use TryValidateObject method to initiate object validation
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            // Check if validation is valid, otherwise throw argument exception
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }

    }
}
