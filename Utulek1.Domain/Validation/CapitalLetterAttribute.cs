using System.ComponentModel.DataAnnotations;

namespace Utulek1.Domain.Validation
{
    // Toto je ten "vlastní validační atribut", který po tobě chtějí
    public class CapitalLetterAttribute : ValidationAttribute
    {
        public CapitalLetterAttribute()
        {
            // Výchozí chybová hláška, pokud ji nezadáš v modelu
            ErrorMessage = "Pole musí začínat velkým písmenem.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Pokud je pole prázdné, necháme to projít (povinnost vyplnění řeší atribut [Required])
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string text = value.ToString()!;

            // Kontrola: Pokud je první znak malé písmeno, vrátíme chybu
            if (char.IsLower(text[0]))
            {
                return new ValidationResult(ErrorMessage);
            }

            // Jinak je vše OK
            return ValidationResult.Success;
        }
    }
}