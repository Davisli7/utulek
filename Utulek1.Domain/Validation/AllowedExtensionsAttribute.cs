using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Utulek1.Domain.Validation
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
            ErrorMessage = $"Povoleny jsou pouze soubory s koncovkami: {string.Join(", ", _extensions)}";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Pokud nic nenahráváme, je to OK (povinnost řeší [Required])
            if (value == null) return ValidationResult.Success;

            // Pomocná metoda pro kontrolu jednoho souboru
            ValidationResult? CheckFile(IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(ErrorMessage);
                }
                return ValidationResult.Success;
            }

            // 1. Pokud je to jeden soubor
            if (value is IFormFile file)
            {
                return CheckFile(file);
            }

            // 2. Pokud je to seznam souborů (IEnumerable<IFormFile>)
            if (value is IEnumerable<IFormFile> files)
            {
                foreach (var f in files)
                {
                    var result = CheckFile(f);
                    if (result != ValidationResult.Success) return result;
                }
            }

            return ValidationResult.Success;
        }
    }
}
