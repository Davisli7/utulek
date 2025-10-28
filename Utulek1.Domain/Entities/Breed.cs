using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    public class Breed
    {
        public int BreedID { get; set; }
        public int SpeciesID { get; set; }
        public string Name { get; set; }
        public Species Species { get; set; }
        public ICollection<Animal> Animals { get; set; }
    }
}
