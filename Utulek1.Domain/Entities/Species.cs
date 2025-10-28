using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    public class Species
    {
        public int SpeciesID { get; set; }
        public string Name { get; set; }
        public ICollection<Breed> Breeds { get; set; }
        public ICollection<Animal> Animals { get; set; }
    }
}
