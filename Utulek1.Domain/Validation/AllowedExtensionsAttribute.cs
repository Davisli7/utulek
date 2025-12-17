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
            if (value == null) return ValidationResult.Success;

            ValidationResult? CheckFile(IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(ErrorMessage);
                }
                return ValidationResult.Success;
            }

            if (value is IFormFile file)
            {
                return CheckFile(file);
            }

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
