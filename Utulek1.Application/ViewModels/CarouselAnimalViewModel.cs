using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Application.ViewModels
{
    public class CarouselAnimalViewModel
    {
        public IList<Carousel>? Carousels { get; set; }
        public IList<Animal>? Animals { get; set; }
    }
}
