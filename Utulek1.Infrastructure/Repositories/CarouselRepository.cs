using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Infrastructure.Repositories
{
    public class CarouselRepository : ICarouselRepository
    {
        private readonly UtulekDbContext _dbContext;

        public CarouselRepository(UtulekDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<Carousel> Select()
        {
            return _dbContext.Carousels.ToList();
        }
    }
}