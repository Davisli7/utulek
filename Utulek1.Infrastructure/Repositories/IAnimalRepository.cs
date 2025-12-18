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
        bool DeletePhoto(int photoId);

        IList<Animal> Select(string? searchName = null, int? speciesId = null, string? status = null);
        Animal? Select(int id);
        void Create(Animal animal);
        void Update(Animal animal);
        bool Delete(int id);

        IList<Species> SelectSpecies();
        IList<Breed> SelectBreeds();
    }
}
