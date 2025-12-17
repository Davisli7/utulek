using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Validation
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInMb;

        public MaxFileSizeAttribute(int maxFileSizeInMb)
        {
            _maxFileSizeInMb = maxFileSizeInMb;
            ErrorMessage = $"Maximální povolená velikost souboru je {_maxFileSizeInMb} MB.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            // Přepočet MB na Byty (1 MB = 1024 * 1024 Bytů)
            long maxBytes = _maxFileSizeInMb * 1024 * 1024;

            ValidationResult? CheckSize(IFormFile file)
            {
                if (file.Length > maxBytes)
                {
                    return new ValidationResult(ErrorMessage);
                }
                return ValidationResult.Success;
            }

            if (value is IFormFile file) return CheckSize(file);

            if (value is IEnumerable<IFormFile> files)
            {
                foreach (var f in files)
                {
                    var result = CheckSize(f);
                    if (result != ValidationResult.Success) return result;
                }
            }

            return ValidationResult.Success;
        }
    }
}
