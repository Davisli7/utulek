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
        void Create(Animal animal);
        bool Delete(int id);
    }
}
