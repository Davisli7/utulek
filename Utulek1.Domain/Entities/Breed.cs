using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    public class Breed
    {
        [Key]
        public int BreedID { get; set; }

        [ForeignKey("Species")]
        [Required]
        public int SpeciesID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public Species Species { get; set; }
        public ICollection<Animal>? Animals { get; set; }
    }
}
