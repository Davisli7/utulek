using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.Abstraction;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure;

namespace Utulek1.Application.Implementation
{
    public class CarouselAppService : ICarouselAppService
    {
        UtulekDbContext _utulekDbContext;

        public CarouselAppService(UtulekDbContext utulekDbContext)
        {
            _utulekDbContext = utulekDbContext;
        }


        public IList<Carousel> Select()
        {
            return _utulekDbContext.Carousels.ToList();
        }
    }
}
