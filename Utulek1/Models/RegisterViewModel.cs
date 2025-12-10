using System.ComponentModel.DataAnnotations;

namespace Utulek1.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Uživatelské jméno je povinné")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email je povinný")]
        [EmailAddress(ErrorMessage = "Neplatný formát emailu")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Jméno je povinné")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Příjmení je povinné")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Heslo je povinné")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Heslo musí mít alespoň 4 znaky")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; }
    }
}
