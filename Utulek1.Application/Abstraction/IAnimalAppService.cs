using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Domain.Entities;

namespace Utulek1.Application.Abstraction
{
    public interface IAnimalAppService
    {
        IList<Animal> Select(string? searchName = null, int? speciesId = null, string? status = null); Animal? Select(int id); 
        IList<Species> SelectSpecies();
        IList<Breed> SelectBreeds();

        void Create(Animal animal, IEnumerable<IFormFile> uploadedFiles);
        void Update(Animal animal, IEnumerable<IFormFile> uploadedFiles); 
        bool DeletePhoto(int photoId);
        bool Delete(int id);
    }
}
