using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.ViewModels;

namespace Utulek1.Application.Abstraction
{
    public interface IHomeService
    {
        CarouselAnimalViewModel GetIndexViewModel();
    }
}
