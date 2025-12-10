using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    public class Animal
    {
        [Key]
        public int AnimalID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Range(0, 50)]
        public int Age { get; set; }

        [ForeignKey("Breed")]
        public int? BreedID { get; set; }
        public Breed? Breed { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [ForeignKey("Species")]
        public int SpeciesID { get; set; }
        public Species? Species { get; set; }

        public ICollection<Photo>? Photos { get; set; }
        public ICollection<AdoptionRequest>? AdoptionRequests { get; set; }
    }
}
