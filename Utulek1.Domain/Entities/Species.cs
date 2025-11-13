using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    public class Species
    {
        [Key]
        public int SpeciesID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Breed>? Breeds { get; set; }
        public ICollection<Animal>? Animals { get; set; }
    }
}
