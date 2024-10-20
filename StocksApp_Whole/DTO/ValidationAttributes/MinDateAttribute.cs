using System.ComponentModel.DataAnnotations;

namespace StocksApp_Whole.DTO.ValidationAttributes
{
    public class MinDateAttribute : ValidationAttribute
    {
        private readonly DateTime _minDate;

        public MinDateAttribute(string minDate)
        {
            _minDate = DateTime.Parse(minDate);
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue >= _minDate)
                {
                    return ValidationResult.Success!;
                }
                else
                {
                    return new ValidationResult($"Date must be on or after {_minDate.ToShortDateString()}.");
                }
            }

            return new ValidationResult("Invalid date format.");
        }
    }
}
