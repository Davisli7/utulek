using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    public class Photo
    {
        public int PhotoID { get; set; }
        public int AnimalID { get; set; }
        public string PhotoURL { get; set; }
        public Animal Animal { get; set; }
    }
}
