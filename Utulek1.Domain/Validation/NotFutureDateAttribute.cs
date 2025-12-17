using System.ComponentModel.DataAnnotations;

namespace Utulek1.Domain.Validation
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        public NotFutureDateAttribute()
        {
            ErrorMessage = "Datum nemůže být v budoucnosti.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date.Date > DateTime.Now.Date)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}