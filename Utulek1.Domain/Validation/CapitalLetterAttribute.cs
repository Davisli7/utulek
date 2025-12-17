using System.ComponentModel.DataAnnotations;

namespace Utulek1.Domain.Validation
{
    public class CapitalLetterAttribute : ValidationAttribute
    {
        public CapitalLetterAttribute()
        {
            ErrorMessage = "Pole musí začínat velkým písmenem.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string text = value.ToString()!;

            if (char.IsLower(text[0]))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}