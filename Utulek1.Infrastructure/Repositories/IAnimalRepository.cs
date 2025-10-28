using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Infrastructure.Repositories
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAllAsync();
        Task<Animal> GetByIdAsync(int id);
        Task AddAsync(Animal animal);
        Task UpdateAsync(Animal animal);
        Task DeleteAsync(int id);
    }
}
