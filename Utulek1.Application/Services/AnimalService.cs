using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure.Repositories;

namespace Utulek1.Application.Services
{
    public class AnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public Task<List<Animal>> GetAllAnimalsAsync() => _animalRepository.GetAllAsync();
        public Task<Animal> GetAnimalByIdAsync(int id) => _animalRepository.GetByIdAsync(id);
        public Task AddAnimalAsync(Animal animal) => _animalRepository.AddAsync(animal);
        public Task UpdateAnimalAsync(Animal animal) => _animalRepository.UpdateAsync(animal);
        public Task DeleteAnimalAsync(int id) => _animalRepository.DeleteAsync(id);
    }
}
