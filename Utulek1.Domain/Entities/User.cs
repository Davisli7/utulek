using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Utulek1.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        // Vlastnosti Email, PasswordHash, Id, UserName JSOU UŽ UVNITŘ IdentityUser
        // Proto je sem NEPIŠ.

        // Tvé vlastní vlastnosti:
        // Otazník 'string?' znamená, že to v databázi může být NULL (nemusí se vyplnit)

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        // CreatedAt chceme povinné, ale dáváme mu defaultní hodnotu, 
        // takže [Required] není třeba (vždy tam něco bude).
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigační vlastnost
        public ICollection<AdoptionRequest>? AdoptionRequests { get; set; }
    }
}
