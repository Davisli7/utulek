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
        IList<Animal> Select();
        Animal? Select(int id); // <--- NOVÁ METODA (načtení jednoho zvířete)
        void Create(Animal animal, IEnumerable<IFormFile> uploadedFiles);
        void Update(Animal animal, IEnumerable<IFormFile> uploadedFiles); // <--- NOVÁ METODA (uložení změn)
        bool Delete(int id);
    }
}
