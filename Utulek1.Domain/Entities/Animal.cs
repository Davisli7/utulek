using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    [Table(nameof(Animal))]
    public class Animal
    {
        public int AnimalID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int? BreedID { get; set; }
        public Breed Breed { get; set; }
        public string Description { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string Status { get; set; } // Available, Adopted, Treatment
        public int SpeciesID { get; set; }
        public Species Species { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<AdoptionRequest> AdoptionRequests { get; set; }
    }
}
