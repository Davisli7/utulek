using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure;
using Utulek1.Infrastructure.Repositories;

namespace Utulek1.Application.Implementation
{
    public class CarouselAppService : ICarouselAppService
    {
        private readonly ICarouselRepository _carouselRepository;
        public CarouselAppService(ICarouselRepository carouselRepository)
        {
            _carouselRepository = carouselRepository;
        }


        public IList<Carousel> Select()
        {
            return _carouselRepository.Select();
        }
    }
}
