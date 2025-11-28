using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    [Table(nameof(Carousel))]
    public class Carousel 
    {
        public int CarouselID { get; set; }
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
    }
}
