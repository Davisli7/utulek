using System.ComponentModel.DataAnnotations;
using Utulek1.Domain.Validation;

namespace Utulek1.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Uživatelské jméno")]
        [Required(ErrorMessage = "Uživatelské jméno je povinné")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} musí mít {2} až {1} znaků.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email je povinný")]
        [EmailAddress(ErrorMessage = "Neplatný formát emailu")]
        public string Email { get; set; }

        [Display(Name = "Křestní jméno")]
        [Required(ErrorMessage = "Jméno je povinné")]
        [CapitalLetter(ErrorMessage = "Jméno musí začínat velkým písmenem")] // <--- VLASTNÍ ATRIBUT
        public string FirstName { get; set; }

        [Display(Name = "Příjmení")]
        [Required(ErrorMessage = "Příjmení je povinné")]
        [CapitalLetter(ErrorMessage = "Příjmení musí začínat velkým písmenem")] // <--- VLASTNÍ ATRIBUT
        public string LastName { get; set; }

        [Display(Name = "Heslo")]
        [Required(ErrorMessage = "Heslo je povinné")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "{0} musí mít alespoň {2} znaky")]
        public string Password { get; set; }

        [Display(Name = "Potvrzení hesla")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; }
    }
}
