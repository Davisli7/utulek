using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    public class AdoptionRequest
    {
        [Key]
        public int RequestID { get; set; }

        [ForeignKey("Animal")]
        public int AnimalID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; }

        [StringLength(500)]
        public string? Message { get; set; }

        public Animal Animal { get; set; }
        public User User { get; set; }
    }

}
